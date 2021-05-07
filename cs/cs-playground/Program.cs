using cs_playground;
using System;
using System.Threading.Tasks;

async void main()
{
    var sg = new Signal<int>();

    async void genData()
    {
        while (!sg.IsClosed)
        {
            Random rnd = new Random();
            sg.Push(rnd.Next(0, 100));
            await Task.Delay(1000);
        }

        Console.WriteLine("Disposed");
    }

    async void subscribeData()
    {
        await foreach (int val in sg)
        {
            Console.WriteLine(val);
        }
        Console.WriteLine("done");
    }

    subscribeData();

    genData();

    await Task.Delay(10000);

    sg.Dispose();
}

main();

Console.ReadKey();