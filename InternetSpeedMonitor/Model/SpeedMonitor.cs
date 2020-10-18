using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace InternetSpeedMonitor.Model
{
    public class SpeedMonitor
    {
        
        private long _PreBytesSent = 0, _PreBytesReceived = 0;

        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);


        public SpeedMonitor()
        {
            Task.Run( () => GetNetworkUsage() );
        }

        public NetworkInterface[] GetAvailableNetworkInterfaces()
        {
            return NetworkInterface.GetAllNetworkInterfaces();
        }

        public void GetNetworkUsage(string NetworkInterface="Wi-Fi")
        {
            IsConnected = IsConnectedToInternet();

            NetworkInterface[] adapters = GetAvailableNetworkInterfaces();
            NetworkInterface networkInterface = null;
            foreach (var i in adapters)
            {
                if (i.Name == NetworkInterface)
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

            NetworkSpeed = (double)(networkInterface.Speed / 10000000.0);
            UploadedData = networkInterface.GetIPv4Statistics().BytesSent / 1024 / 1024;
            DownloadedData = networkInterface.GetIPv4Statistics().BytesReceived / 1024 / 1024;
            DownloadSpeed = Math.Round((double)dSpeed, 2);
            UploadSpeed = Math.Round((double)uSpeed, 2);

        }

        public bool IsConnectedToInternet()
        {
            if (!CheckNet())
            {
                try
                {
                    using (var client = new WebClient())
                    using (var stream = client.OpenRead("https://google.com/generate_204"))
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
            return true;

        }

        public static bool CheckNet()
        {
            int desc;
            return InternetGetConnectedState(out desc, 0);
        }


        public double DownloadedData { get; set; }
        public double UploadedData { get; set; }
        public double NetworkSpeed { get; set; }
        public double DownloadSpeed { get; set; }
        public double UploadSpeed { get; set; }
        public bool IsConnected { get; private set; }

    }
}
