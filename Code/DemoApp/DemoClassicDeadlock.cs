using System;
using System.Diagnostics;
using System.Threading;

namespace DemoApp
{
    /// <summary>
    ///     Nested locks
    /// </summary>
    internal class DemoClassicDeadlock
    {
        private readonly object _lockForResourceA = new object();
        private readonly object _lockForResourceB = new object();

        public void Run()
        {
            var w1 = new Thread(Worker1);
            var w2 = new Thread(Worker2);

            // TODO use names to locate the threads in the debugger!
            //var w1 = new Thread(Worker1){Name = "Worker 1"};
            //var w2 = new Thread(Worker2){Name = "Worker 2"};

            w1.Start();
            w2.Start();

            w1.Join();
            w2.Join();
        }

        public void Worker1()
        {
            lock (_lockForResourceA)
            {
                Thread.Sleep(100);
                lock (_lockForResourceB)
                {
                    // We need both locks
                    // For example copying values from resource A to B
                    Do_Work_With_ResourceA_and_ResourceB();
                }
            }
        }

        public void Worker2()
        {
            lock (_lockForResourceB)
            {
                Thread.Sleep(100);
                lock (_lockForResourceA)
                {
                    // We need both locks
                    // For example copying values from resource B to A
                    Do_Work_With_ResourceA_and_ResourceB();
                }
            }
        }

        private void Do_Work_With_ResourceA_and_ResourceB()
        {
            Trace.WriteLine("Beginning work with both resources.");
            Thread.Sleep(1000);
            Trace.WriteLine("Work with both resources done.");
        }

        #region Solution with timeouts

        public void RunSolutionWithTimeouts()
        {
            var done = false;
            while (!done)
            {
                try
                {
                    if (Monitor.TryEnter(_lockForResourceA, TimeSpan.FromMilliseconds(100)))
                    {
                        if (Monitor.TryEnter(_lockForResourceB, TimeSpan.FromMilliseconds(100)))
                        {
                            Do_Work_With_ResourceA_and_ResourceB();
                            done = true;
                        }
                    }
                }
                finally
                {
                    // Give up
                    if (Monitor.IsEntered(_lockForResourceA))
                    {
                        Monitor.Exit(_lockForResourceA);
                    }

                    if (Monitor.IsEntered(_lockForResourceB))
                    {
                        Monitor.Exit(_lockForResourceB);
                    }
                }
            }
        }

        #endregion
    }
}