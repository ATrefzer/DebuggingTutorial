using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
// ReSharper disable All

namespace DemoApp
{
    /// <summary>
    ///     Example taken from here: https://michaelscodingspot.com/c-deadlocks-in-depth-part-2/
    ///
    ///     - Disable Ui
    ///     - Run background worker
    ///     - Endable Ui (Needs to be called in UI Thread again)
    /// 
    /// </summary>
    internal class DemoDispatcherQueueDeadlock
    {
        private readonly Dispatcher _uiDispatcher;

        public DemoDispatcherQueueDeadlock(Dispatcher uiDispatcher)
        {
            _uiDispatcher = uiDispatcher;
        }

        /// <summary>
        /// Called when the button is clicked.
        /// </summary>
        public void Run()
        {
            // Called from UI thread
            DisableUI();
            Task.Run(LongRunningWork).Wait();
        }

        private void LongRunningWork()
        {
            // Running in a thread pool thread.
            Thread.Sleep(3000);

            _uiDispatcher.Invoke(UpdateUI);

            // Remaining code ...
        }

        private void UpdateUI()
        {
            // What we do here does not matter. But we want to do it in the UI thread.
            EnableUI();
        }

        private void DisableUI()
        {
            // Not implemented
        }

        private void EnableUI()
        {
            // Not implemented
        }

        private void Asserts()
        {
            #region Asserts

            Debug.Assert(ReferenceEquals(Application.Current.Dispatcher, _uiDispatcher) is true);

            // Creates a new dispatcher if no one exists for the current threads.
            Debug.Assert(ReferenceEquals(Dispatcher.CurrentDispatcher, _uiDispatcher) is false);
            Debug.Assert(ReferenceEquals(Dispatcher.FromThread(Thread.CurrentThread), _uiDispatcher) is false);

            #endregion
        }
    }
}