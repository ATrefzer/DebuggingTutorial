using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DemoApp
{
    /// <summary>
    ///     Not shown in the slides.
    /// 
    ///     I think the original code was from Joe Duffy' book?
    ///     https://stackoverflow.com/questions/21723880/property-with-volatile-or-lock
    ///     http://www.albahari.com/threading/part4.aspx#_The_volatile_keyword
    ///     Can the condition _a == 0 && _b == 0 happen?
    /// </summary>
    internal class DemoReordering
    {
        /// <summary>
        ///     Disable optimizations.
        ///     Other threads sees modifications immediately. No caching. Still reordering.
        /// </summary>
        private volatile int _a, _b, _x, _y;

        public void Run()
        {
            for (var i = 0; i < 1000000; i++) // 1 million
            {
                var t1 = Task.Run(A);
                var t2 = Task.Run(B);
                Task.WaitAll(t1, t2);

                if (_a == 0 && _b == 0)
                {
                    MessageBox.Show($"a={_a}, b={_b}");
                }

                _a = _b = _x = _y = 0;
            }
        }

        private void A()
        {
            _x = 1;
            //Interlocked.MemoryBarrier();
            _a = _y;
            
            // Reasoning:
            // If _y is 0 then B() did not run yet and _x is already set to 1.
            // So when B() is executed _x is already 1. 
        }

        private void B()
        {
            _y = 1;
            //Interlocked.MemoryBarrier();
            _b = _x;
        }
    }
}