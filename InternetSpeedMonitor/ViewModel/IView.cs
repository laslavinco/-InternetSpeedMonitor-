using System.Windows.Controls;

namespace InternetSpeedMonitor.ViewModel
{
    public interface IView
    {
        string GetCurrentSelectedNetworkInterface();
        void UpdateSelectedNetworkInterface();
        Image NetworkConnectionIcon { get; set; }
    }
}
