﻿using InternetSpeedMonitor.ViewModel;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace InternetSpeedMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool clicked = false;
        private Point lmAbs = new Point();

        public MainWindow()
        {
            if (IsWindowOpen("InternetSpeedMonitor"))
                ShutdownApp();
            InitializeComponent();
            
            this.DataContext = new InternetSpeedMonitorViewModel();
            this.MouseDown += PnMouseDown;
            this.MouseUp += PnMouseUp;
            this.MouseMove += PnMouseMove;
            SetMainWindowPosition();
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

        public static bool IsConnectedToInternet()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public void CheckPing()
        {
            if (!IsConnectedToInternet())
                NetworkConnectionIcon.Source = (ImageSource)FindResource("Disconnected");
            else
                NetworkConnectionIcon.Source = (ImageSource)FindResource("Connected");
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

        private void DownloadTextBlock_TargetUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            TextBlock downloadTextBlock = sender as TextBlock;
            if ( Int32.Parse(downloadTextBlock.Text) == 0)
                CheckPing();
        }
    }
}
