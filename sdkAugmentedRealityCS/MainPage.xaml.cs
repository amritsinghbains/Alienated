using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace sdkAugmentedRealityCS
{
    public partial class Page2 : PhoneApplicationPage
    {
      
        public Page2()
        {
            InitializeComponent();
            mainSound.AutoPlay = true;
            mainSound.Play();
            
  
        }
       

        private void image1_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
           
            levelopener1.Begin(); NavigationService.Navigate(new Uri("/MainPage2.xaml?level=1", UriKind.Relative));                       
        }
        private void image2_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            levelopener2.Begin(); NavigationService.Navigate(new Uri("/MainPage2.xaml?level=2", UriKind.Relative)); 
        }
        private void image3_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            levelopener3.Begin(); NavigationService.Navigate(new Uri("/MainPage2.xaml?level=3", UriKind.Relative)); 
        }
        private void image4_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            levelopener4.Begin(); NavigationService.Navigate(new Uri("/MainPage2.xaml?level=4", UriKind.Relative)); 
        }
        private void image5_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            levelopener5.Begin(); NavigationService.Navigate(new Uri("/MainPage2.xaml?level=5", UriKind.Relative)); 
        }
        private void image6_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            levelopener6.Begin(); NavigationService.Navigate(new Uri("/MainPage2.xaml?level=6", UriKind.Relative)); 
        }
        private void image7_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            levelopener7.Begin(); NavigationService.Navigate(new Uri("/HowToPlay.xaml", UriKind.Relative)); 
        }

        private void soundToggle(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.sound == 1)
            {
                App.sound = 0;
                mainSound.AutoPlay = false;
                mainSound.Stop();
                soundImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Images/soundoff.png", UriKind.Relative));

            }
            else
                if(App.sound==0)
            {
                App.sound = 1; 
                mainSound.AutoPlay = true;
                mainSound.Play();
                soundImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Images/soundon.png", UriKind.Relative));

            }
        }
    }
}