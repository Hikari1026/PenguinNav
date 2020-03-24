using System;
using System.Windows;
using System.Windows.Threading;

namespace PenguinNav
{
    public partial class Splash : Window
    {
        DispatcherTimer dt = new DispatcherTimer();


        public Splash()
        {
            InitializeComponent();
            dt.Tick += new EventHandler(dt_Tick);
            dt.Interval = new TimeSpan(0, 0, 2);
            dt.Start();
        }

        private void dt_Tick (object sender, EventArgs e)
        {
            Launcher launcher = new Launcher();
            launcher.Show();

            dt.Stop();
            this.Close();
        }

    }
}
