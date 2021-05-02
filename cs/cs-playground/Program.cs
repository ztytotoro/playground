using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

async void main()
{
    var sg = new Signal<int>();

    async void genData()
    {
        while(!sg.IsClosed)
        {
            Random rnd = new Random();
            sg.Push(rnd.Next(0, 100));
            await Task.Delay(1000);
        }
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

public class Signal<T>
{
    private readonly CancellationTokenSource _cts = new();
    private TaskCompletionSource<T> result { get; set; }

    public bool IsClosed { get => _cts.IsCancellationRequested; }

    public Signal()
    {
        result = new TaskCompletionSource<T>(_cts.Token);
        result.SetCanceled(_cts.Token);
    }

    public void Push(T val)
    {
        result.SetResult(val);
    }

    private async IAsyncEnumerable<T> GetData()
    {
        while (!_cts.IsCancellationRequested)
        {
            var val = await result.Task;
            yield return val;
            result = new TaskCompletionSource<T>();
            result.SetCanceled(_cts.Token);
        }
    }

    public IAsyncEnumerator<T> GetAsyncEnumerator()
    {
        return GetData().GetAsyncEnumerator().WithCancellation(_cts.Token);
    }

    public void Dispose()
    {
        _cts.Cancel();
    }
}