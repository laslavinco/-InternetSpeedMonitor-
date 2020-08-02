using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace InternetSpeedMonitor.Model
{
    public class SpeedMonitor
    {
        
        private long _PreBytesSent = 0, _PreBytesReceived = 0;

        public SpeedMonitor()
        {
            Task.Run((Action)GetNetworkUsage);
        }

        public void GetNetworkUsage()
        {
            NetworkInterface networkInterface = null;
            foreach (var i in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (i.Name == "Wi-Fi")
                {
                    networkInterface = i;
                    break;
                }
            }

            if (networkInterface is null)
                return;

            IPv4InterfaceStatistics stats = networkInterface.GetIPv4Statistics();
            long uSpeed = (stats.BytesSent - _PreBytesSent) / 1024;
            long dSpeed = (stats.BytesReceived - _PreBytesReceived) / 1024;

            _PreBytesSent = networkInterface.GetIPv4Statistics().BytesSent;
            _PreBytesReceived = networkInterface.GetIPv4Statistics().BytesReceived;

            DownloadSpeed = Math.Round((double)dSpeed, 2);
            UploadSpeed = Math.Round((double)uSpeed, 2);

        }


        public double DownloadSpeed { get; set; }
        public double UploadSpeed { get; set; }

        public long PreBytesSent { get => _PreBytesSent; set => _PreBytesSent = value; }
    }
}
