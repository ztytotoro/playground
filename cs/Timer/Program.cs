using System;
using System.Collections.Generic;
using System.Threading;

var queue = new Queue<int>();

var tm = new Timer(_ =>
{
    for (var i = 0; i < 1; i++)
    {
        queue.Enqueue(0);
    }
}, null, 0, 125);

var t2 = new Timer(_ =>
{
    Console.WriteLine(queue.Count);
}, null, 0, 1000);

Console.ReadKey();