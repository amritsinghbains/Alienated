using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Phone.Controls;

using Microsoft.Devices.Sensors;
using Microsoft.Devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Matrix = Microsoft.Xna.Framework.Matrix;
using System.Threading;
using System.Windows.Media.Imaging;

using Microsoft.Xna.Framework.Media;


namespace sdkAugmentedRealityCS
{
    public partial class MainPage : PhoneApplicationPage
    {
        int score;
        int enemy;
        int enemyKilled;
        int flag;
        Motion motion;
        PhotoCamera cam;

        List<Image> textBlocks;
        List<Vector3> points;
     

        Viewport viewport;
        Matrix projection;
        Matrix view;
        int noCamera = 0;
        System.Windows.Threading.DispatcherTimer dt, dt2;


        // Constructor
        public MainPage()
        {

            InitializeComponent();
          


            // Initialize the list of TextBlock and Vector3 objects.
            textBlocks = new List<Image>();
            points = new List<Vector3>();
            score = 0;
            enemy = 0;
            enemyKilled=0;
            flag = 0;
        }
       

        public void InitializeViewport()
        {
            // Initialize the viewport and matrixes for 3d projection.
            viewport = new Viewport(0, 0, (int)this.ActualWidth, (int)this.ActualHeight);
            float aspect = viewport.AspectRatio;
            projection = Matrix.CreatePerspectiveFieldOfView(1, aspect, 1, 12);
            view = Matrix.CreateLookAt(new Vector3(0, 0, 1), Vector3.Zero, Vector3.Up);
        }

        string level_game;
        int level_game_no;
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            
            if (NavigationContext.QueryString.TryGetValue("level", out level_game))
            {
                level_game_no = Convert.ToInt32(level_game);
                if (!(level_game_no > 0 && level_game_no <= 6))
                    level_game_no = 1;
              

            }



            // Initialize the camera and set the video brush source.
            cam = new Microsoft.Devices.PhotoCamera();
            viewfinderBrush.SetSource(cam);

            if (!Motion.IsSupported)
            {
                MessageBox.Show("the Motion API is not supported on this device. Sorry your device is not supported. ");

                NavigationService.Navigate(new Uri("/gameover.xaml?score=0&enemykilled=0&level=1", UriKind.Relative));

                noCamera = 1;
                        
                return;
            }

            // If the Motion object is null, initialize it and add a CurrentValueChanged
            // event handler.
            if (motion == null)
            {
                motion = new Motion();
                motion.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);
                motion.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<MotionReading>>(motion_CurrentValueChanged);
            }

            // Try to start the Motion API.
            try
            {
                motion.Start();
            }
            catch (Exception)
            {
                MessageBox.Show("unable to start the Motion API.");
                NavigationService.Navigate(new Uri("/gameover.xaml?score=0&enemykilled=0&level=1", UriKind.Relative));

                noCamera = 1;
                
            }

            // Hook up the event handler for when the user taps the screen.
           // this.MouseLeftButtonUp += new MouseButtonEventHandler(MainPage_MouseLeftButtonUp);

           // AddDirectionPoints();

            base.OnNavigatedTo(e);
           


            //calculate level
            int createAliens=10000,increaseAliens=200;
            switch(level_game_no){
             case 1: createAliens = 10000; increaseAliens = 200; break;
             case 2: createAliens = 7000; increaseAliens = 150; break;
             case 3: createAliens = 4000; increaseAliens = 125; break;
             case 4: createAliens = 2500; increaseAliens = 100; break;
             case 5: createAliens = 2000; increaseAliens = 75; break;
             case 6: createAliens = 1500; increaseAliens = 50; break;
            
                default: createAliens = 10000 ; increaseAliens = 200; break;
            }

         
            //call cow
            dt= new System.Windows.Threading.DispatcherTimer();
            dt.Interval = new TimeSpan(0, 0, 0, 0, createAliens); // time to create new aliens
            dt.Tick += new EventHandler(AddDirectionPoints);
            dt.Start();

            //textblockincreaser
            dt2 = new System.Windows.Threading.DispatcherTimer();
            dt2.Interval = new TimeSpan(0, 0, 0, 0, increaseAliens); // time to grow aliens
            dt2.Tick += new EventHandler(textBlockIncreaser);
            dt2.Start();
        }


       
        private void textBlockIncreaser(object sender, EventArgs e)
        {
           

                        for (int i = 0; i < points.Count; i++)
                          {
                              if (!textBlocks[i].Name.Contains("dead"))
                              {
                                  textBlocks[i].Width += 3;
                                  textBlocks[i].Height += 3;
                               

                              }

                              if (flag == 0)
                              {
                                  if (textBlocks[i].Width >= 450)
                                  {


                                    

                                      NavigationService.Navigate(new Uri("/gameover.xaml?score=" + score + "&enemykilled=" + enemyKilled + "&level=" + level_game_no, UriKind.Relative));
                                      flag = 1;
                                   


                                  }
                              }
                            
                         }

        }


        
/*
        void MainPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // If the Canvas containing the TextBox is visible, ignore
            // this event.
            if (TextBoxCanvas.Visibility == Visibility.Visible)
            {
                return;
            }

            // Save the location where the user touched the screen.
            pointOnScreen = e.GetPosition(LayoutRoot);

            // Save the device attitude when the user touched the screen.
            attitude = motion.CurrentValue.Attitude.RotationMatrix;

            // Make the Canvas containing the TextBox visible and
            // give the TextBox focus.
            TextBoxCanvas.Visibility = Visibility.Visible;
            NameTextBox.Focus();
        }


        */
        void motion_CurrentValueChanged(object sender, SensorReadingEventArgs<MotionReading> e)
        {
            // This event arrives on a background thread. Use BeginInvoke
            // to call a method on the UI thread.
            Dispatcher.BeginInvoke(() => CurrentValueChanged(e.SensorReading));
        }



        private void CurrentValueChanged(MotionReading reading)
        {
            if (viewport.Width == 0)
            {
                InitializeViewport();
            }


            // Get the RotationMatrix from the MotionReading.
            // Rotate it 90 degrees around the X axis to put it in xna coordinate system.
            Matrix attitude = Matrix.CreateRotationX(MathHelper.PiOver2) * reading.Attitude.RotationMatrix;

            // Loop through the points in the list
            for (int i = 0; i < points.Count; i++)
            {
                // Create a World matrix for the point.
                Matrix world = Matrix.CreateWorld(points[i], new Vector3(0, 0, 1), new Vector3(0, 1, 0));

                // Use Viewport.Project to project the point from 3D space into screen coordinates.
                Vector3 projected = viewport.Project(Vector3.Zero, projection, view, world * attitude);


                if (projected.Z > 1 || projected.Z < 0)
                {
                    // If the point is outside of this range, it is behind the camera.
                    // So hide the TextBlock for this point.
                    textBlocks[i].Visibility = Visibility.Collapsed;
                }
                else
                {
                    // Otherwise, show the TextBlock
                    textBlocks[i].Visibility = Visibility.Visible;

                    // Create a TranslateTransform to position the TextBlock.
                    // Offset by half of the TextBlock's RenderSize to center it on the point.
                    TranslateTransform tt = new TranslateTransform();
                    tt.X = projected.X - (textBlocks[i].RenderSize.Width / 2);
                    tt.Y = projected.Y - (textBlocks[i].RenderSize.Height / 2);
                    textBlocks[i].RenderTransform = tt;
                }
            }
        }


        private void AddPoint(Vector3 point, string name)
        {
            // Create a new TextBlock. Set the Canvas.ZIndexProperty to make sure
            // it appears above the camera rectangle.
            enemy++;
            enemyDisplay.Text = "ENEMY: " + enemy;

            score += level_game_no * 50;
            scoreDisplay.Text = "SCORE: " + score;
            Image textblock = new Image();

            Random rnd = new Random();
            int chosen = rnd.Next(1, level_game_no+1);
          
                BitmapImage bi3 = new BitmapImage();
               
            bi3.UriSource = new Uri("/Images/enemy_aliens/"+chosen+".png", UriKind.Relative);
               
            textblock.Stretch = Stretch.Fill;
            textblock.Source = bi3;
            textblock.Name = name;
            textblock.Width = 30;
            textblock.Height = 30;
            textblock.SetValue(Canvas.ZIndexProperty, 2);
            textblock.Visibility = Visibility.Collapsed;

            // Add the TextBlock to the LayoutRoot container.
            LayoutRoot.Children.Add(textblock);

            // Add the TextBlock and the point to the List collections.
            textBlocks.Add(textblock);
            points.Add(point);

            textblock.Tap += TextBlockClicked;

        }
        private void TextBlockClicked(object sender, RoutedEventArgs e)
        {
            
            enemyKilled++;
          
            enemy--;
            enemyDisplay.Text = "ENEMY: " + enemy;

            score += level_game_no * 100;

            scoreDisplay.Text = "SCORE: " + score;
            Image a = (Image)e.OriginalSource;
//            a.Text = "*";
         //   textBlocks.Remove(a);
            a.Name = "dead";
            a.Width = 0;
            a.Height = 0;
        }

               /*increase the size of textblock

            tempTextBlock = textblock;
            System.Windows.Threading.DispatcherTimer dt = new System.Windows.Threading.DispatcherTimer();
            dt.Interval = new TimeSpan(0, 0, 0, 0, 1000); // 3000 Milliseconds
            dt.Tick += new EventHandler(textBlockIncrease);
            dt.Start();
            

        
        TextBlock tempTextBlock;
        private void textBlockIncrease(object sender, EventArgs e)
        {
            tempTextBlock.FontSize += 5;
        }
                * */
        static int alienNumber=1;
        private void AddDirectionPoints(object sender, EventArgs e)
        {
            

          
            Random rnd = new Random();
            int randomNo1 = rnd.Next(-5, 5);
            int randomNo2 = rnd.Next(-1,5);
            int randomNo3 = rnd.Next(-5,5);



            if (randomNo1 != 0 && randomNo2 != 0 && randomNo3 != 0)
            {
//                AddPoint(new Vector3(randomNo1, randomNo2, randomNo3), alienNumber + "");
                AddPoint(new Vector3(randomNo1, randomNo2, randomNo3), randomNo1 + "," + randomNo2 + "," + randomNo3);

                alienNumber++;
            }

           
         //   AddPoint(new Vector3(0, 0, -10), "front");
           // AddPoint(new Vector3(0, 0, 10), "back");
           // AddPoint(new Vector3(10, 0, 0), "right");
          //  AddPoint(new Vector3(-10, 0, 0), "left");
          //  AddPoint(new Vector3(0, 10, 0), "top");
          //  AddPoint(new Vector3(0, -10, 0), "bottom");
        }
        /*
        private void NameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // When the TextBox loses focus. Hide the Canvas containing it.
            TextBoxCanvas.Visibility = Visibility.Collapsed;
        }
         */


        /*
        private void NameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            // If the key is not the Enter key, don't do anything.
            if (e.Key == Key.Enter)
            {
                return;
            }

            // When the TextBox loses focus. Hide the Canvas containing it.
            TextBoxCanvas.Visibility = Visibility.Collapsed;

            // If any of the objects we need are not present, exit the event handler.
            if (NameTextBox.Text == "" || pointOnScreen == null || motion == null)
            {
                return;
            }

            // Translate the point before projecting it.
            System.Windows.Point p = pointOnScreen;
            p.X = LayoutRoot.RenderSize.Width - p.X;
            p.Y = LayoutRoot.RenderSize.Height - p.Y;
            p.X *= .5;
            p.Y *= .5;


            // Use the attitude Matrix saved in the OnMouseLeftButtonUp handler.
            // Rotate it 90 degrees around the X axis to put it in xna coordinate system.
            attitude = Matrix.CreateRotationX(MathHelper.PiOver2) * attitude;

            // Use Viewport.Unproject to translate the point on the screen to 3D space.
            Vector3 unprojected = viewport.Unproject(new Vector3((float)p.X, (float)p.Y, -.9f), projection, view, attitude);
            unprojected.Normalize();
            unprojected *= -10;

            // Call the helper method to add this point
            AddPoint(unprojected, p.X + "," + p.Y);

          


            // Clear the TextBox
            NameTextBox.Text = "";
        }
        */
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {

            if (noCamera == 0)
            {
                dt.Stop();
                dt2.Stop();
            }

            // Dispose camera to minimize power consumption and to expedite shutdown.
            cam.Dispose();
            base.OnNavigatedFrom(e);
         
        }
    }
}
