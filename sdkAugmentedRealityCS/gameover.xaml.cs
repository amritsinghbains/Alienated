using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace sdkAugmentedRealityCS
{
    public partial class Page1 : PhoneApplicationPage
    {

        string score, enemykilled, level;
        public Page1()
        {
            InitializeComponent();
            
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {



            if (NavigationService.CanGoBack)
                NavigationService.RemoveBackEntry();


            
            if (NavigationContext.QueryString.TryGetValue("score", out score))
            {
              
                scoreDisplay.Text = score;
                
            }


            if (NavigationContext.QueryString.TryGetValue("enemykilled", out enemykilled))
            {
               
                killDisplay.Text = enemykilled;
                
            }
            if (NavigationContext.QueryString.TryGetValue("level", out level))
            {

              

            }




        }
        private void email_Click(object sender, EventArgs e)
        {
            EmailComposeTask task = new EmailComposeTask();
            task.Subject = "Alienated Score on Level "+level;
            task.Body = "I played Alienated on Windows Phone, and scored " + scoreDisplay.Text + " with " + killDisplay.Text + " kills on Level "+level+" Link: http://www.windowsphone.com/en-in/store/app/alienated/515e3ee0-5c81-4ac7-a772-d91345d554b0";
            task.Show();

        }

        private void fb_Click(object sender, EventArgs e)
        {
            ShareLinkTask shareLinkTask = new ShareLinkTask();
            shareLinkTask.LinkUri = new Uri("http://www.windowsphone.com/en-in/store/app/alienated/515e3ee0-5c81-4ac7-a772-d91345d554b0", UriKind.Absolute);
            shareLinkTask.Message = "I played Alienated on Windows Phone, and scored " + scoreDisplay.Text + " with " + killDisplay.Text + " kills on Level " + level;
           
            shareLinkTask.Show();


        }

        private void twitter_Click(object sender, EventArgs e)
        {
            ShareLinkTask shareLinkTask = new ShareLinkTask();
            shareLinkTask.LinkUri = new Uri("http://www.windowsphone.com/en-in/store/app/alienated/515e3ee0-5c81-4ac7-a772-d91345d554b0", UriKind.Absolute);
            shareLinkTask.Message = "I played Alienated on Windows Phone, and scored " + scoreDisplay.Text + " with " + killDisplay.Text + " kills on Level " + level;
            shareLinkTask.Show();


        }
        private void rating_Click(object sender, EventArgs e)
        {
            //code
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();

            marketplaceReviewTask.Show();

        }

        private void PlayAgain_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
            else
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }


        }
    
    }
}