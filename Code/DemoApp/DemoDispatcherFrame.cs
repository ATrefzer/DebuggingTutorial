using System.Windows.Threading;

namespace DemoApp
{
    /// <summary>
    ///     There is no button for this demo in the UI
    /// </summary>
    internal class DemoDispatcherFrame
    {
        public void DoEvents()
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(ExitFrame),
                frame);
            Dispatcher.PushFrame(frame);
        }

        public object ExitFrame(object f)
        {
            ((DispatcherFrame) f).Continue = false;
            return null;
        }
    }
}