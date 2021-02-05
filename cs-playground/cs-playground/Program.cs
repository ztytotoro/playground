using System;
using System.Collections.Generic;
using System.Threading.Tasks;

async void main()
{
    var sg = new Signal();

    async void genData()
    {
        while(!sg.isClosed)
        {
            Random rnd = new Random();
            sg.Push(rnd.Next(0, 100));
            await Task.Delay(1000);
        }
    }

    async void subscribeData()
    {
        await foreach (int val in sg.GetData())
        {
            Console.WriteLine(val);
        }
        Console.WriteLine("done");
    }

    subscribeData();

    genData();

    await Task.Delay(10000);

    sg.dispose();
}

main();

Console.ReadKey();

public class Signal
{
    private TaskCompletionSource<int> result = new TaskCompletionSource<int>();
    private bool closed = false;

    public bool isClosed { get => closed; }

    public void Push(int val)
    {
        result.SetResult(val);
    }

    public async IAsyncEnumerable<int> GetData()
    {
        while (!closed)
        {
            int? val = null;
            try
            {
                val = await result.Task;
            }
            catch (Exception)
            {
                Console.WriteLine("canceled");
            }
            if(!closed && val is not null)
            {
                yield return (int)val;
                result = new TaskCompletionSource<int>();
            }
        }
    }

    public void dispose()
    {
        closed = true;
        result.SetCanceled();
    }
}