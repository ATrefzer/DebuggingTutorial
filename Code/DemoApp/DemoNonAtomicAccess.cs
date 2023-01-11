using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DemoApp
{
    /// <summary>
    ///     From here: https://coders-corner.net/2013/01/12/multithreading-in-c-teil-2-atomare-datenzugriffe/
    /// </summary>
    internal class DemoNonAtomicAccess
    {
        // 10 millions
        private readonly int _loopCount = 10000000;
        private int _failed;
        private long _int64Value;

        /// <summary>
        ///     Compile as x86 or x64 to get different results.
        ///     But even for x86 the error is very small. Not comparable to Guid.
        /// </summary>
        public void Run()
        {
            _failed = 0;

            _int64Value = 1000000000000000001;
            Parallel.Invoke(SetValue, GetValue);

            MessageBox.Show(
                $"Failed = {_failed} Correct = {100.0 * _failed / _loopCount}%");
        }

        private void SetValue()
        {
            for (var i = 0; i < _loopCount; i++)
            {
                if (i % 2 == 0)
                {
                    _int64Value = 1000000000000000001;
                }
                else
                {
                    _int64Value = 4000000000000000004;
                }
            }
        }

        private void GetValue()
        {
            for (var i = 0; i < _loopCount; i++)
            {
                var actualValue = _int64Value;

                if (actualValue == 1000000000000000001)
                {
                }
                else if (actualValue == 4000000000000000004)
                {
                }
                else
                {
                    Interlocked.Increment(ref _failed);
                }
            }
        }
    }
}