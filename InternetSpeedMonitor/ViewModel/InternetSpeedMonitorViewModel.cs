using InternetSpeedMonitor.Model;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using System.Windows.Media;

namespace InternetSpeedMonitor.ViewModel
{
    class InternetSpeedMonitorViewModel : INotifyPropertyChanged
    {
        private SpeedMonitor _speedMonitor;
        private static Timer _timer;
        private double _downloadSpeed;
        private double _uploadSpeed;
        private double _networkSpeed;
        private double _downloadedData;
        private double _uploadedData;
        private bool _isConnected;
        private NetworkInterface[] _networkInterfaces;
        private string _seleectedInterface;

        public event PropertyChangedEventHandler PropertyChanged;

        public InternetSpeedMonitorViewModel()
        {
            _speedMonitor = new SpeedMonitor();
            SetTimer();
        }

        private void SetStats()
        {
            _speedMonitor.GetNetworkUsage(_seleectedInterface);
            NetworkInterfaces = NetworkInterfaces ?? _speedMonitor.GetAvailableNetworkInterfaces();
            SelectedInterface = SelectedInterface ?? NetworkInterfaces.First().Name;
            DownloadSpeed = _speedMonitor.DownloadSpeed;
            UploadSpeed = _speedMonitor.UploadSpeed;
            NetworkSpeed = _speedMonitor.NetworkSpeed;
            DownloadedData = _speedMonitor.DownloadedData;
            UploadedData = _speedMonitor.UploadedData;
            IsConnected = _speedMonitor.IsConnected;
        }

        private void SetTimer()
        {
            _timer = new Timer(1000);
            _timer.Elapsed += (sender, e) => { SetStats(); } ;
            _timer.AutoReset = true;
            _timer.Enabled = true;
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

        private ImageSource FindResource(string resourceKey)
        {

            try
            {
                return (ImageSource)Application.Current.MainWindow.FindResource(resourceKey);
            }
            catch
            {
                return null;
            }
        }

        public double DownloadedData
        {
            get
            {
                return _downloadedData; 
            }
            set
            {
                _downloadedData = value;
                OnPropertyChanged();
            }
        }

        public double UploadedData
        {
            get
            {
                return _uploadedData;
            }
            set
            {
                _uploadedData = value;
                OnPropertyChanged();
            }
        }


        public double NetworkSpeed
        {
            get
            {
                return _networkSpeed;
            }
            set
            {
                _networkSpeed = value;
                OnPropertyChanged();
            }
        }

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

        public bool IsConnected 
        { 
            get
            {
                return _isConnected;
            }
            set
            {
                _isConnected = value;
                OnPropertyChanged();
                OnPropertyChanged("ConnectionStatus");
            }
        }

        public ImageSource ConnectionStatus
        {
            get => FindResource(IsConnected ? "Connected" : "Disconnected");
        }

        public NetworkInterface[] NetworkInterfaces
        {
            get => _networkInterfaces;
            set
            {
                _networkInterfaces = value;
                OnPropertyChanged();
            }

        }

        public string SelectedInterface
        {
            get => _seleectedInterface;
            set
            {
                _seleectedInterface = value;
                OnPropertyChanged();
            }
        }
    }
}
