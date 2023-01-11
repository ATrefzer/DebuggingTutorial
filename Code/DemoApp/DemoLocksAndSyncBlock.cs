using System.Threading;

namespace DemoApp
{
    internal class DemoLocksAndSyncBlock
    {
        private readonly object _lockForResourceA = new object();
        private readonly object _lockForResourceB = new object();

        public void RunSyncBlockDemo()
        {
            lock (_lockForResourceA) // Sync Block Index
            {
                // Code inside here can only be executed
                // by one thread at a time.
                // Other threads have to wait
            }

            // Call GetHashCode to occupy the object header
            var code = _lockForResourceB.GetHashCode();
            lock (_lockForResourceB)
            {
                // Code inside here can only be executed
                // by one thread at a time.
                // Other threads have to wait
            }
        }

        public void LockStatementResolvedByCompiler()
        {
            // lock statement resolved by the compiler
            try
            {
                Monitor.Enter(_lockForResourceA);
            }
            finally
            {
                Monitor.Exit(_lockForResourceA);
            }
        }
    }
}