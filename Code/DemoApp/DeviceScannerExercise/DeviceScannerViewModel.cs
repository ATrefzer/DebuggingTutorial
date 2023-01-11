using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using DemoApp.Annotations;
using Prism.Commands;

namespace DemoApp.DeviceScannerExercise
{
    /// <summary>
    ///     Deadlock exercise.
    /// </summary>
    internal sealed class DeviceScannerViewModel : INotifyPropertyChanged
    {
        private readonly DeviceScanner _deviceScanner = new DeviceScanner();
        private readonly Dispatcher _uiDispatcher;

        private ReadOnlyCollection<int> _devices;
        private bool _isEnabled = true;
        private ImageSource _status;

        public DeviceScannerViewModel(Dispatcher uiDispatcher)
        {
            _uiDispatcher = uiDispatcher;
            _deviceScanner.NewDevicesAvailable += OnNewDevicesAvailable;
            ScanForDevicesCommand = new DelegateCommand(ScanForDevicesAsync);
        }

        public ICommand ScanForDevicesCommand { get; set; }

        public ReadOnlyCollection<int> Devices
        {
            get => _devices;
            set
            {
                _devices = value;
                OnPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public ImageSource Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        
        private async void ScanForDevicesAsync()
        {
            IsEnabled = false;
            try
            {
                Status = null;
                await _deviceScanner.ScanAsync();
            }
            finally
            {
                IsEnabled = true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnNewDevicesAvailable(object sender, EventArgs e)
        {
            // Called from a worker thread. Update the Ui with all found devices.
            _uiDispatcher.Invoke(UpdateDevicesForUi);
        }

        private void UpdateStatusImage()
        {
            // In WPF creating the bitmap has to occur in the UI thread.

            var uri = new Uri("pack://application:,,,/Ok.png");
            var bitmap = new BitmapImage(uri);
            Status = bitmap;
        }

        private void UpdateDevicesForUi()
        {
            UpdateStatusImage();
            Devices = _deviceScanner.GetFoundDevicesAddresses();
        }


        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}