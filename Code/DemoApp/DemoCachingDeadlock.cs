using System.Threading;
using System.Windows;

namespace DemoApp
{
    /// <summary>
    ///     http://www.albahari.com/threading/part4.aspx#_The_volatile_keyword
    ///     Run in release mode! Without debugger attached
    ///     Resolve via lock or volatile keyword.
    /// </summary>
    internal class DemoCachingDeadlock
    {
        private bool IsComplete { get; set; }


        public void Run()
        {
            var t = new Thread(ThreadFunc);

            t.Start();
            Thread.Sleep(1000);
            IsComplete = true;
            t.Join(); // Blocks indefinitely
        }

        private void ThreadFunc()
        {
            var toggle = false;

            while (!IsComplete)
            {
                toggle = !toggle;
            }

            MessageBox.Show("done");
        }
    }
}