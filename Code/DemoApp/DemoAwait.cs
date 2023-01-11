using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DemoApp
{
    // https://www.youtube.com/watch?v=RqJESGHEMDY
    // https://www.youtube.com/watch?v=wErS2ODmkNQ
    internal class DemoAwait
    {
        public async void Run()
        {
            var awaiter = Task.Delay(TimeSpan.FromMilliseconds(1)).GetAwaiter();

            // Anything which provides a GetAwaiter method can be behind an await.
            await Task.Delay(TimeSpan.FromMilliseconds(1));


            // Interface is not needed. Only access to a method called GetAwaiter
            await TimeSpan.FromMilliseconds(10);

            // Home grown  awaitable object
            var sleep = new Sleepy();
            await sleep;
        }
    }

    public static class AwaitExtensions
    {
        public static TaskAwaiter GetAwaiter(this TimeSpan timespan)
        {
            return Task.Delay(timespan).GetAwaiter();
        }
    }

    // Making process awaitable
    public static class ProcessAwaiter
    {
        public static TaskAwaiter<int> GetAwaiter(this Process process)
        {
            var tcs = new TaskCompletionSource<int>();
            process.EnableRaisingEvents = true;
            process.Exited += (s, e) => tcs.TrySetResult(process.ExitCode);
            if (process.HasExited)
            {
                tcs.TrySetResult(process.ExitCode);
            }

            return tcs.Task.GetAwaiter();
        }
    }


    public class Sleepy
    {
        public MyAwaiter GetAwaiter()
        {
            return new MyAwaiter(this);
        }

        public Task SleepAsnc()
        {
            return Task.Delay(100);
        }
    }


    public class MyAwaiter : INotifyCompletion
    {
        private readonly Task task;

        public MyAwaiter(Sleepy sleepy)
        {
            task = sleepy.SleepAsnc();
        }

        public bool IsCompleted => task.IsCompleted;

        public void OnCompleted(Action continuation)
        {
            task.ContinueWith(t => continuation);
        }

        public void GetResult()
        {
            task.GetAwaiter().GetResult();
        }
    }
}