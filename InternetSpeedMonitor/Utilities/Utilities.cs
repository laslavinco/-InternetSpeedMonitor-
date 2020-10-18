using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace InternetSpeedMonitor.Utilities
{
    static class Utilities
    {
        [DllImport("wininet.dll")]
        public extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        public static bool GetInternetState()
        {
            bool returnValue = false;
            try
            {

                int Desc;
                returnValue = InternetGetConnectedState(out Desc, 0);

            }
            catch
            {
                returnValue = false;
            }
            return returnValue;
            
        }

        public static List<string> ConvertNetworkInterfaceToString(NetworkInterface[] networkInterfaces)
        {

            List<string> NetworkInterfacesList = new List<string>();

            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                NetworkInterfacesList.Add(networkInterface.Name);
            }

            return NetworkInterfacesList;

        }

    }
}
