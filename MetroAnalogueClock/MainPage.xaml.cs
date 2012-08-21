using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;



namespace MetroAnalogueClock
{
   
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            Layout(Display);
            timer.Tick += (object sender, object e) =>
            {
                 time = DateTime.Now;
                SecondHand(time.Second);
                MinuteHand(time.Minute, time.Second);
                HourHand(time.Hour, time.Minute, time.Second);
            };
            timer.Start();
        }

        private DispatcherTimer timer = new DispatcherTimer();
        private DateTime time;
        private Canvas markers = new Canvas();
        private Canvas face = new Canvas();
        private Rectangle secondsHand;
        private Rectangle minutesHand;
        private Rectangle hoursHand;
        private double diameter;
        private int secondsWidth = 1;
        private int secondsHeight;
        private int minutesWidth = 5;
        private int minutesHeight;
        private int hoursWidth = 8;
        private int hoursHeight;
        private Brush faceBackground = ((Brush)App.Current.Resources["FaceBackgroundBrush"]);
        private Brush faceForeground = ((Brush)App.Current.Resources["FaceForegroundBrush"]);
        private Brush SecondHandBackGround = ((Brush)App.Current.Resources["SecondHandBrush"]);
        private Brush MinuteHandBackGround = ((Brush)App.Current.Resources["MinuteHandBrush"]);
        private Brush HourHandBackGround = ((Brush)App.Current.Resources["HourHandBrush"]);
        private Brush rimForeground = ((Brush)App.Current.Resources["RimForegroundBrush"]);
        private Brush rimBackground = ((Brush)App.Current.Resources["RimBackgroundBrush"]);

        private Rectangle Hand(double width, double height,
                Brush background, double radiusX, double radiusY, double thickness)
        {
            Rectangle hand = new Rectangle();
            hand.Width = width;
            hand.Height = height;
            hand.Fill = background;
            hand.StrokeThickness = thickness;
            hand.RadiusX = radiusX;
            hand.RadiusY = radiusY;
            return hand;
        }

        private void RemoveHand(ref Rectangle hand)
        {
            if (hand != null && face.Children.Contains(hand))
            {
                face.Children.Remove(hand);
            }
        }

        private void AddHand(ref Rectangle hand)
        {
            if (!face.Children.Contains(hand))
            {
                face.Children.Add(hand);
            }
        }

        private TransformGroup TransformGroup(double Angle, double X, double Y)
        {
            TransformGroup transformGroup = new TransformGroup();
            TranslateTransform firstTranslate = new TranslateTransform();
            firstTranslate.X = X;
            firstTranslate.Y = Y;
            transformGroup.Children.Add(firstTranslate);
            RotateTransform rotateTransform = new RotateTransform();
            rotateTransform.Angle = Angle;
            transformGroup.Children.Add(rotateTransform);
            TranslateTransform secondTranslate = new TranslateTransform();
            secondTranslate.X = diameter / 2;
            secondTranslate.Y = diameter / 2;
            transformGroup.Children.Add(secondTranslate);
            return transformGroup;
        }

        private void SecondHand(int seconds)
        {
            RemoveHand(ref secondsHand);
            
                secondsHand = Hand(secondsWidth, secondsHeight, SecondHandBackGround, 0, 0, 0);
                secondsHand.RenderTransform = TransformGroup(seconds * 6, 0, -secondsHeight);
                AddHand(ref secondsHand);
                  }

        private void MinuteHand(int minutes, int seconds)
        {
            RemoveHand(ref minutesHand);
            
                minutesHand = Hand(minutesWidth, minutesHeight, MinuteHandBackGround, 2, 2, 0.6);
                minutesHand.RenderTransform = TransformGroup(6 * minutes + seconds / 10,
                0, -minutesHeight);
                AddHand(ref minutesHand);
           
        }

        private void HourHand(int hours, int minutes, int seconds)
        {
            RemoveHand(ref hoursHand);
            
                hoursHand = Hand(hoursWidth, hoursHeight, HourHandBackGround, 3, 3, 0.6);
                hoursHand.RenderTransform = TransformGroup(30 * hours + minutes / 2 + seconds / 120,
                0, -hoursHeight);
                AddHand(ref hoursHand);
            
        }

        private void Layout(Canvas canvas)
        {
            Ellipse rim = new Ellipse();
            Ellipse inner = new Ellipse();
            canvas.Children.Clear();
            diameter = canvas.Width;
            rim.Height = diameter;
            rim.Width = diameter;
            rim.Fill = rimBackground;
            canvas.Children.Add(rim);
            inner.Width = diameter - 40;
            inner.Height = diameter - 40;
            inner.Fill = faceBackground;
            Canvas.SetTop(inner, 20);
            Canvas.SetLeft(inner, 20);
            canvas.Children.Add(inner);
            markers.Children.Clear();
            markers.Width = diameter;
            markers.Height = diameter;
            for (int i = 0; i < 60; i++)
            {
                Rectangle marker = new Rectangle();
                marker.Fill = rimForeground;
                if ((i % 5) == 0)
                {

                    marker.Width = 3;
                    marker.Height = 8;
                    marker.RenderTransform = TransformGroup(i * 6, 0, (diameter-40)/2 + (20 - marker.Height)/2  );
                }
                else
                {
                    marker.Width = 1;
                    marker.Height = 4;
                    marker.RenderTransform = TransformGroup(i * 6, 0,
                    (diameter - 40) / 2 + (20 - marker.Height) / 2);
                }
                markers.Children.Add(marker);
            }
            canvas.Children.Add(markers);
            face.Width = diameter;
            face.Height = diameter;
            canvas.Children.Add(face);
            secondsHeight = (int)diameter / 2 - 20;
            minutesHeight = (int)diameter / 2 - 40;
            hoursHeight = (int)diameter / 2 - 80;
        }

        
        
    }
}
