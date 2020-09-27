using System.Threading;
using System.Threading.Tasks;

namespace DeribitDotNet.Utils
{
    public sealed class AsyncManualResetEvent
    {
        private TaskCompletionSource<bool> _taskCompletionSource = new TaskCompletionSource<bool>();

        public Task WaitTask() => _taskCompletionSource.Task;

        public void Set() => _taskCompletionSource.TrySetResult(true);

        public void Reset()
        {
            TaskCompletionSource<bool> tcs;

            do
            {
                tcs = _taskCompletionSource;
            } while (tcs.Task.IsCompleted && Interlocked.CompareExchange(ref _taskCompletionSource,
                         new TaskCompletionSource<bool>(), tcs) != tcs);
        }
    }
}