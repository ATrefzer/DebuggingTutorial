using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DemoApp
{
    /// <summary>
    ///     https://blog.stephencleary.com/2016/12/eliding-async-await.html
    /// </summary>
    internal class DemoAsyncAwaitPitfalls
    {
        private static readonly AsyncLocal<int> context = new AsyncLocal<int>();

        public Task SwallowExceptions()
        {
            // Do not await it. Just return the task. Saves the state machine.
            //return Task.Delay(1);


            // However ....


            try
            {
                // ... here it would be wrong.
                // You return the task that contains the exception and so you are no longer in the try block(!)
                return ThrowWithAsync();
            }
            catch (Exception e)
            {
                // Not called
                Console.WriteLine(e);
            }


            try
            {
                // If this is an void fire and forget we return immediately
                // and leave the catch handler (!)
                // The exception in the task (there is no) in not unpacked.
                FireAndForget();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


            // This exception is just disappearing(!)
            // The Exception is contained in the task.
            ThrowWithAsync();


            // We can use this in event handlers.
            // Don't Swallow the exception but handle it.
            ThrowWithAsync().SafeFireAndForget();

            using (var client = new HttpClient())
            {
                // If we elide the await here the request is cancelled because the using scope is exited
                // and the client disposed before the request is finished.
                //return client.GetStringAsync("url");
            }


            return Task.CompletedTask;
        }


        private async void FireAndForget()
        {
            await Task.Delay(1).ConfigureAwait(false);
        }


        /// <summary>
        ///     Shows that Wait throws an AggregateException, GetAwaiter.GetResult the original exception.
        /// </summary>
        public async Task AvoidAggregateException()
        {
            try
            {
                ThrowWithAsync().Wait();
            }
            catch (Exception ex)
            {
                // Aggregate Exception
                MessageBox.Show($"Exception of type: {ex.GetType().Name} was thrown!");
            }

            try
            {
                // InvalidCastException. GetAwaiter.GetResult returns the original exception.
                ThrowWithAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception of type: {ex.GetType().Name} was thrown!");
            }

            try
            {
                await ThrowWithAsync();
            }
            catch (Exception ex)
            {
                // InvalidCastException
                MessageBox.Show($"Exception of type: {ex.GetType().Name} was thrown!");
            }
        }

        private async Task ThrowWithAsync()
        {
            await Task.Delay(1000).ConfigureAwait(false); // Get rid of the warning

            // The exception is put into the task.
            // It is actually thrown inside the state machine.
            throw new InvalidCastException();
        }

        private Task ThrowWithoutAsync()
        {
            // The exception is thrown directly and not contained in the task.
            throw new InvalidCastException();
        }

        private static async Task MainAsync()
        {
            context.Value = 1;
            Console.WriteLine("Should be 1: " + context.Value);
            await Async();
            Console.WriteLine("Should be 1: " + context.Value);
        }

        private static async Task Async()
        {
            Console.WriteLine("Should be 1: " + context.Value);
            context.Value = 2;
            Console.WriteLine("Should be 2: " + context.Value);
            await Task.Yield();
            Console.WriteLine("Should be 2: " + context.Value);
        }

        public void TrySomething()
        {
            CallSyncMethod();
        }

        private async void CallSyncMethod()
        {
            await ThrowWithAsync();
        }
    }


    public static class AsyncExtensions
    {
        public static async void SafeFireAndForget(this Task task, bool continueOnCapturedContext = true,
            Action<Exception> onException = null)
        {
            try
            {
                await task.ConfigureAwait(continueOnCapturedContext);
            }
            catch (Exception e) when (onException != null)
            {
                onException(e);
                throw;
            }
        }
    }
}