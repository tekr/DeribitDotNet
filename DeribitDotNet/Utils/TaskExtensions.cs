using System;
using System.Threading.Tasks;

namespace DeribitDotNet.Utils
{
    public static class TaskExtensions
    {
        public static async Task TimeoutAfter(this Task task, int timeoutMs)
        {
            if (await Task.WhenAny(task, Task.Delay(timeoutMs)) != task)
            {
                throw new TimeoutException($"Operation timed out after {timeoutMs} ms");
            }
        }
    }
}
