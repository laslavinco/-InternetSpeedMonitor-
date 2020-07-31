using InternetSpeedMonitor.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;

namespace InternetSpeedMonitor.ViewModel
{
    class InternetSpeedMonitorViewModel : INotifyPropertyChanged
    {
        private SpeedMonitor _speedMonitor;
        private static Timer _timer;
        public event PropertyChangedEventHandler PropertyChanged;

        public InternetSpeedMonitorViewModel()
        {
            _speedMonitor = new SpeedMonitor();
            _speedMonitor.GetNetworkUsage();
            DownloadSpeed = _speedMonitor.DownloadSpeed;
            UploadSpeed = _speedMonitor.UploadSpeed;
            SetTimer();
        }

        private void SetTimer()
        {
            _timer = new Timer(1000);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            _speedMonitor.GetNetworkUsage();
            DownloadSpeed = _speedMonitor.DownloadSpeed;
            UploadSpeed = _speedMonitor.UploadSpeed;
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != null)
            {
                OnPropertyChanged(e.PropertyName);
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == null)
                return;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        private double _downloadSpeed;
        private double _uploadSpeed;

        public double DownloadSpeed
        {   get
            {
                return _downloadSpeed;
            }
            set
            {
                _downloadSpeed = value;
                OnPropertyChanged();
            }
        }

        public double UploadSpeed
        {
            get
            {
                return _uploadSpeed;
            }

            set
            {
                _uploadSpeed = value;
                OnPropertyChanged();
            }
        }


    }
}
