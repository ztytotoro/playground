using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace cs_playground
{
    public class Signal<T>
    {
        private readonly AsyncSubject<T> _subject = new();
        private readonly CancellationTokenSource _cts = new();
        public bool IsClosed { get => _cts.IsCancellationRequested; }

        public void Push(T val)
        {
            _subject.Append(val);
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return _subject.ToAsyncEnumerable().GetAsyncEnumerator().WithCancellation(_cts.Token);
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
            _subject.Dispose();
        }
    }
}
