using InternetSpeedMonitor.ViewModel;
using System;
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

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new InternetSpeedMonitorViewModel();
            this.MouseDown += PnMouseDown;
            this.MouseUp += PnMouseUp;
            this.MouseMove += PnMouseMove;
            GetTaskBarPosition();
        }

        public int GetTaskBarPosition()
        {
            int position = 0;
            double width = System.Windows.SystemParameters.WorkArea.Width;
            double height = System.Windows.SystemParameters.WorkArea.Height;
            this.Left = width - (width * 8) / 100;
            this.Top = height - (height * 10) / 100;
            return position;
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
    }
}
