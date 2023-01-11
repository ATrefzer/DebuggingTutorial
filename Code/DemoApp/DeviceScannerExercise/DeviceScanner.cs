using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace DemoApp.DeviceScannerExercise
{
    /// <summary>
    /// Scans for devices for x seconds.
    /// An NewDevicesAvailable event is fired for each intermediate result.
    /// You can request the list of found devices via GetFoundDevicesAddresses (final or intermediate)
    /// </summary>
    internal sealed class DeviceScanner
    {
        private readonly object _lock = new object();
        private readonly Random _rnd = new Random();
        private List<int> _deviceAddresses;

        public event EventHandler NewDevicesAvailable;

        public Task ScanAsync()
        {
            lock (_lock)
            {
                _deviceAddresses = new List<int>();
            }

            return Task.Run(DoScan);
        }

        private void DoScan()
        {
            // Try find new devices for 5 seconds
            const int timeoutMs = 5000;
            var sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < timeoutMs)
            {
                Thread.Sleep(_rnd.Next(10) * 100);

                var deviceAddress = _rnd.Next(256);

                lock (_lock)
                {
                    _deviceAddresses.Add(deviceAddress);

                    // Update UI with intermediate results.
                    OnResultAvailable();
                }
            }
        }

        private void OnResultAvailable()
        {
            NewDevicesAvailable?.Invoke(this, EventArgs.Empty);
        }

        public ReadOnlyCollection<int> GetFoundDevicesAddresses()
        {
            lock (_lock)
            {
                var copy = new List<int>(_deviceAddresses);
                return copy.AsReadOnly();
            }
        }
    }
}