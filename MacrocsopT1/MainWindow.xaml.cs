﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Threading;
using System.ComponentModel;

namespace MacrocsopT1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string url;
        static string place;

        private Image currentImage;
        private Button currentStop, currentStart;

        public MainWindow()
        {
            InitializeComponent();
            string iniPlace = AppDomain.CurrentDomain.BaseDirectory + "фон.jpg";
            FirstImage.Source = new BitmapImage(new Uri(iniPlace));
            SecondImage.Source = new BitmapImage(new Uri(iniPlace));
            ThirdImage.Source = new BitmapImage(new Uri(iniPlace));
        }

        WebClient webClient1, webClient2, webClient3;
        BitmapImage bmi1, bmi2, bmi3;
        private void Download(int imageNumber, int bmiNumber, string _url, string _place, WebClient webClient)
        {
            try
            {
                Uri link = new Uri(_url);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressChanged);
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted);
                webClient.QueryString.Add("imageNumber", imageNumber.ToString());
                webClient.QueryString.Add("bmiNumber", bmiNumber.ToString());
                webClient.DownloadFileAsync(link, _place);
                currentStop.IsEnabled = true;
                currentStart.IsEnabled = false;
            }
            catch (Exception)
            {
                MessageBox.Show("Wrong URL...", "Wait, what..?", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            OverallPB.Value = e.ProgressPercentage;
        }

        //private BitmapImage InitializingBMI(BitmapImage bitmapImage, string source)
        //{
        //    bitmapImage = new BitmapImage();
        //    bitmapImage.BeginInit();
        //    bitmapImage.UriSource = new Uri(source);
        //    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        //    bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache | BitmapCreateOptions.DelayCreation;
        //    bitmapImage.EndInit();
        //    return bitmapImage;
        //}

        private void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled == false)
            {
                WebClient obj = sender as WebClient;
                int bmiNumber = Int32.Parse(obj.QueryString["bmiNumber"]);
                switch (bmiNumber)
                {
                    case 1:
                        place = AppDomain.CurrentDomain.BaseDirectory + "first.jpg";
                        bmi1 = new BitmapImage();
                        bmi1.BeginInit();
                        bmi1.UriSource = new Uri(place);
                        bmi1.EndInit();
                        break;
                    case 2:
                        place = AppDomain.CurrentDomain.BaseDirectory + "second.jpg";
                        bmi2 = new BitmapImage();
                        bmi2.BeginInit();
                        bmi2.UriSource = new Uri(place);
                        bmi2.EndInit();
                        break;
                    case 3:
                        place = AppDomain.CurrentDomain.BaseDirectory + "third.jpg";
                        bmi3 = new BitmapImage();
                        bmi3.BeginInit();
                        bmi3.UriSource = new Uri(place);
                        bmi3.EndInit();
                        break;
                    default:
                        break;
                }
                //BitmapImage bmi = new BitmapImage();
                //bmi.BeginInit();
                //bmi.UriSource = new Uri(place);
                //bmi.CacheOption = BitmapCacheOption.OnLoad;
                //bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache; //| BitmapCreateOptions.DelayCreation;
                //bmi.EndInit();

                int imageNumber = Int32.Parse(obj.QueryString["imageNumber"]);
                switch (imageNumber)
                {
                    case 1:
                        FirstImage.Source = bmi1;
                        Start1Btn.IsEnabled = true;
                        Stop1Btn.IsEnabled = false;
                        break;
                    case 2:
                        SecondImage.Source = bmi2;
                        Start2Btn.IsEnabled = true;
                        Stop2Btn.IsEnabled = false;
                        break;
                    case 3:
                        ThirdImage.Source = bmi3;
                        Start3Btn.IsEnabled = true;
                        Stop3Btn.IsEnabled = false;
                        break;
                }
            }
        }
        private void Start1Btn_Click(object sender, RoutedEventArgs e)
        {
            OverallPB.Value = 0;
            url = FirstImageTb.Text;
            place = AppDomain.CurrentDomain.BaseDirectory + "first.jpg";
            webClient1 = new WebClient();
            if (!string.IsNullOrEmpty(url))
            {
                currentImage = FirstImage;
                currentStop = Stop1Btn;
                currentStart = Start1Btn;
                Download(1, 1, url, place, webClient1);
            }
        }

        private void Filling(TextBox tb)
        {
            if (tb.Text.Contains(" ") || tb.Text == "")
                tb.Text = "Enter URL...";
        }
        private void FirstImageTb_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FirstImageTb.Clear();
            Filling(SecondImageTb);
            Filling(ThirdImageTb);
        }

        private void Start2Btn_Click(object sender, RoutedEventArgs e)
        {
            OverallPB.Value = 0;
            url = SecondImageTb.Text;
            place = AppDomain.CurrentDomain.BaseDirectory + "second.jpg";
            webClient2 = new WebClient();
            if (!string.IsNullOrEmpty(url))
            {
                currentImage = SecondImage;
                currentStop = Stop2Btn;
                currentStart = Start2Btn;
                Download(2, 2, url, place, webClient2);
            }
        }

        private void Start3Btn_Click(object sender, RoutedEventArgs e)
        {
            OverallPB.Value = 0;
            url = ThirdImageTb.Text;
            place = AppDomain.CurrentDomain.BaseDirectory + "third.jpg";
            webClient3 = new WebClient();
            if (!string.IsNullOrEmpty(url))
            {
                currentImage = ThirdImage;
                currentStop = Stop3Btn;
                currentStart = Start3Btn;
                Download(3, 3, url, place, webClient3);
            }
        }

        private void SecondImageTb_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SecondImageTb.Clear();
            Filling(FirstImageTb);
            Filling(ThirdImageTb);
        }

        private void ThirdImageTb_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ThirdImageTb.Clear();
            Filling(FirstImageTb);
            Filling(SecondImageTb);
        }

        private  void StartAllBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(FirstImageTb.Text) && (FirstImageTb.Text != "Enter URL...") &&
                !string.IsNullOrEmpty(SecondImageTb.Text) && (SecondImageTb.Text != "Enter URL...") &&
                !string.IsNullOrEmpty(ThirdImageTb.Text) && (ThirdImageTb.Text != "Enter URL..."))
            {
                currentStart = Start1Btn;
                currentStop = Stop1Btn;
                place = AppDomain.CurrentDomain.BaseDirectory + "first.jpg";
                Download(1, 1, FirstImageTb.Text, place, webClient1 = new WebClient());

                currentStart = Start2Btn;
                currentStop = Stop2Btn;
                place = AppDomain.CurrentDomain.BaseDirectory + "second.jpg";
                Download(2, 2, SecondImageTb.Text, place, webClient2 = new WebClient());

                currentStart = Start3Btn;
                currentStop = Stop3Btn;
                place = AppDomain.CurrentDomain.BaseDirectory + "third.jpg";
                Download(3, 3, ThirdImageTb.Text, place, webClient3 = new WebClient());
            }
            else
            {
                MessageBox.Show("Please enter all three URL addresses correctly...", "Wait, what..?", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Stop1Btn_Click(object sender, RoutedEventArgs e)
        {
            webClient1.CancelAsync();
            Stop1Btn.IsEnabled = false;
            Start1Btn.IsEnabled = true;
        }
        private void Stop2Btn_Click(object sender, RoutedEventArgs e)
        {
            webClient2.CancelAsync();
            Stop2Btn.IsEnabled = false;
            Start2Btn.IsEnabled = true;
        }

        private void Stop3Btn_Click(object sender, RoutedEventArgs e)
        {
            webClient3.CancelAsync();
            Stop3Btn.IsEnabled = false;
            Start3Btn.IsEnabled = true;
        }
    }
}

