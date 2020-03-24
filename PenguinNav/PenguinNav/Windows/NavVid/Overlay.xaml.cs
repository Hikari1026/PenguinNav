using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace PenguinNav.Windows.NavVid
{
    /// <summary>
    /// Logica di interazione per Overlay.xaml
    /// </summary>
    public partial class Overlay : UserControl, INotifyPropertyChanged
    {
        //Gui variables

        #region LeftSliderAngle
        private double myLeftSliderAngle = 0;

        public double LeftSliderAngle {
            get { return myLeftSliderAngle; }
            set { myLeftSliderAngle = value; NotifyPropertyChanged(); }
        }
        #endregion

        #region RightSliderAngle
        private double myRightSliderAngle = 0;

        public double RightSliderAngle {
            get { return myRightSliderAngle; }
            set { myRightSliderAngle = value; NotifyPropertyChanged(); }
        }
        #endregion

        #region GyroIndicator
        private double myGyroIndicator = 0;

        public double GyroIndicator {
            get { return myGyroIndicator; }
            set { myGyroIndicator = value; NotifyPropertyChanged(); }
        }
        #endregion

        public Overlay()
        {
            InitializeComponent();
            DataContext = this;
        }

        #region EventHandlers
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion


        //DEBUG ONLY --- NEED TO BIND TO CONTROLLER INPUT
        #region SlidersFunctions
        private void LSValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LeftSliderAngle = LSValue.Value;
        }

        private void RSValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RightSliderAngle = RSValue.Value;
        }

        private void GVaue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GyroIndicator = GVaue.Value;
        }
        #endregion


        //SideMenu open
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Storyboard storyboard = Resources["OpenMenu"] as Storyboard;
            storyboard.Begin(SideMenu);
        }

        //SideMenu close
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Storyboard storyboard = Resources["CloseMenu"] as Storyboard;
            storyboard.Begin(SideMenu);
        }
    }
}
