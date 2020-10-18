using InternetSpeedMonitor.ViewModel;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace InternetSpeedMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool clicked = false;
        private Point lmAbs = new Point();
        private System.Windows.Forms.NotifyIcon ni;
        public MainWindow()
        {
            if (IsWindowOpen("InternetSpeedMonitor"))
                ShutdownApp();
            InitializeComponent();

            this.SetupSystemTrayIcon();

            this.DataContext = new InternetSpeedMonitorViewModel();
            this.MouseDown += PnMouseDown;
            this.MouseUp += PnMouseUp;
            this.MouseMove += PnMouseMove;
            SetMainWindowPosition();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
        }

        private void SetupSystemTrayIcon()
        {
            System.Windows.Forms.NotifyIcon icon = new System.Windows.Forms.NotifyIcon();
            icon.Icon = new System.Drawing.Icon(Application.GetResourceStream(new Uri("pack://application:,,,/Resources/appIcon.ico")).Stream);
            ni = new System.Windows.Forms.NotifyIcon();
            ni.Icon = new System.Drawing.Icon(Application.GetResourceStream(new Uri("pack://application:,,,/Resources/appIcon.ico")).Stream);

            ni.Visible = true;
            ni.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    var x = sender as System.Windows.Forms.NotifyIcon;
                    x.Visible = false;
                    this.Show();
                    this.WindowState = WindowState.Normal;
                };
        }

        
        private void ShutdownApp()
        {
            Application.Current.Shutdown();
        }

        public static bool IsWindowOpen(string processName)
        {
            Process[] allProcess = Process.GetProcessesByName(processName);
            return (allProcess.Count() > 1);
        }

        public void SetMainWindowPosition()
        {
            double systemWidth = SystemParameters.WorkArea.Width;
            double systemHeight = SystemParameters.WorkArea.Height;
            double appWidth = this.Width;
            double appHeight = this.Height;

            this.Left = systemWidth - appWidth;
            this.Top = systemHeight - appHeight;

        }

        void PnMouseDown(object sender, System.Windows.Input.MouseEventArgs e)
        {
            clicked = true;
            this.lmAbs = e.GetPosition(this);
            this.lmAbs.Y = Convert.ToInt16(this.Top) + this.lmAbs.Y;
            this.lmAbs.X = Convert.ToInt16(this.Left) + this.lmAbs.X;
        }

        void PnMouseUp(object sender, System.Windows.Input.MouseEventArgs e)
        {
            clicked = false;
        }

        void PnMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (clicked)
            {
                Point MousePosition = e.GetPosition(this);
                Point MousePositionAbs = new Point();
                MousePositionAbs.X = Convert.ToInt16(this.Left) + MousePosition.X;
                MousePositionAbs.Y = Convert.ToInt16(this.Top) + MousePosition.Y;
                this.Left = this.Left + (MousePositionAbs.X - this.lmAbs.X);
                this.Top = this.Top + (MousePositionAbs.Y - this.lmAbs.Y);
                this.lmAbs = MousePositionAbs;
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            this.ni.Visible = true;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var messageBoxResutlts = MessageBox.Show(Application.Current.MainWindow, "Are you sure you want to close ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResutlts == MessageBoxResult.Yes)
                ShutdownApp();
        }

        private void PinOnTopButtonClicked(object sender, RoutedEventArgs e)
        {
            this.Topmost = !(this.Topmost);
        }

    }
}
