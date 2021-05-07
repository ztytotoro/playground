using System.Collections.Generic;
using System.Threading.Tasks;

namespace cs_playground
{
    public class Signal<T>
    {
        private readonly Queue<ValueTask<T>> _resultQueue = new();
        private readonly Queue<ManualResetValueTaskSource<T>> _sourceQueue = new();
        private bool _closed = false;

        public bool IsClosed => _closed;

        public void Push(T val)
        {
            CheckQueue();
            var source = _sourceQueue.Dequeue();
            source.SetResult(val);
        }

        public async IAsyncEnumerator<T> GetAsyncEnumerator()
        {
            while(!_closed)
            {
                CheckQueue();
                var task = _resultQueue.Dequeue();
                yield return await task;
            }
        }

        public void Dispose()
        {
            _closed = true;
        }

        private void CheckQueue()
        {
            if (_sourceQueue.Count == 0)
            {
                var source = new ManualResetValueTaskSource<T>();
                var task = new ValueTask<T>(source, 0);
                _sourceQueue.Enqueue(source);
                _resultQueue.Enqueue(task);
            }
        }
    }
}
