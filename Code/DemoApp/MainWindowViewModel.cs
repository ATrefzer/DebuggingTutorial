using System;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using DemoApp.DeviceScannerExercise;
using Prism.Commands;
using ThirdPartyLibrary;

namespace DemoApp
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private ImageSource _image;

        public ICommand ClassicDeadlockCommandTimeoutSolution { get; set; }

        public ICommand ClassicDeadlockCommand { get; set; }

        public ICommand SyncBlockCommand { get; set; }

        public ICommand CrashCommand { get; set; }

        public ICommand DispatcherQueueDeadlockCommand { get; set; }

        public ICommand CreateImageCommand { get; set; }

        public ICommand ReorderCommand { get; set; }

        public ICommand FirstAndSecondChanceCommand { get; }

        public ICommand NonAtomicAccessCommand { get; }

        public ICommand CachingDeadlockCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            ClassicDeadlockCommand = new DelegateCommand(ExecuteClassicDeadlock);
            ClassicDeadlockCommandTimeoutSolution = new DelegateCommand(ExecuteClassicDeadlockTimeoutSolution);
            SyncBlockCommand = new DelegateCommand(ExecuteSyncBlock);
            CrashCommand = new DelegateCommand(ExecuteCrash);
            DispatcherQueueDeadlockCommand = new DelegateCommand(ExecuteDispatcherDeadlock);
            CreateImageCommand = new DelegateCommand(ExecuteCreateImage);
            ScannerViewModel = new DeviceScannerViewModel(Dispatcher.CurrentDispatcher);
            ReorderCommand = new DelegateCommand(ExecuteReorder);
            FirstAndSecondChanceCommand = new DelegateCommand(ExecuteFirstAndSecondChanceCommand);
            NonAtomicAccessCommand = new DelegateCommand(ExecuteNonAtomicAccess);
            CachingDeadlockCommand = new DelegateCommand(ExecuteCachingDeadlock);
            AvoidAggregateExceptionCommand = new DelegateCommand(ExecuteAvoidAggregateException);
            SwallowingExceptionsCommand = new DelegateCommand(async () => await ExecuteSwallowingExceptions());
            TrySomethingCommand = new DelegateCommand(ExecuteTrySomething);
        }

        private void ExecuteTrySomething()
        {
            var demo = new DemoAsyncAwaitPitfalls();
            demo.TrySomething();
        }


        private async Task ExecuteSwallowingExceptions()
        {
            var demo = new DemoAsyncAwaitPitfalls();
            await demo.SwallowExceptions();
        }

        private async void ExecuteAvoidAggregateException()
        {
            var demo = new DemoAsyncAwaitPitfalls();
            await demo.AvoidAggregateException();

        }

        public DeviceScannerViewModel ScannerViewModel { get; set; }

        public ImageSource Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }

        public ICommand AvoidAggregateExceptionCommand
        {
            get;
        }

        public ICommand SwallowingExceptionsCommand
        {
            get;
        }

        public ICommand TrySomethingCommand
        {
            get;
        }

        private void ExecuteNonAtomicAccess()
        {
            var demo = new DemoNonAtomicAccess();
            demo.Run();
        }

        private void ExecuteFirstAndSecondChanceCommand()
        {
            try
            {
                throw new ArgumentException();
            }
            catch (ArgumentException)
            {
                // TODO Explain why this code is not ok.
                throw new UserException("This crashes the application");
            }
        }

        private void ExecuteReorder()
        {
            var demo = new DemoReordering();
            demo.Run();
        }

        private void ExecuteCreateImage()
        {
            //Image = CreateBitMapNoLocking();
            var demo = new DemoRenderDeadlock();
            Image = demo.CreateBitMapDirectBackbufferWrite();
        }

        private void ExecuteDispatcherDeadlock()
        {
            // Use the dispatcher assigned to this thread: UI Thread
            // Calling Dispatcher.CurrentDispatcher from another thread returns another dispatcher or no dispatcher at all!
            var demo = new DemoDispatcherQueueDeadlock(Dispatcher.CurrentDispatcher);
            demo.Run();
        }

        private void ExecuteCachingDeadlock()
        {
            var demo = new DemoCachingDeadlock();
            demo.Run();
        }

        private void ExecuteCrash()
        {
            var lib = new ThirdPartyService();
            lib.Initialize();

            MessageBox.Show("No crash");
        }

        private void ExecuteClassicDeadlockTimeoutSolution()
        {
            var demo = new DemoClassicDeadlock();
            demo.RunSolutionWithTimeouts();
        }

        private void ExecuteSyncBlock()
        {
            var demo = new DemoLocksAndSyncBlock();
            demo.RunSyncBlockDemo();
        }

        private void ExecuteClassicDeadlock()
        {
            var demo = new DemoClassicDeadlock();
            demo.Run();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}