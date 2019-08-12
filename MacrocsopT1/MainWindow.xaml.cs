using System;
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
        }
        
        WebClient webClient1;
        WebClient webClient2;
        WebClient webClient3;
        private void Download(int imageNumber, string _url, string _place, WebClient webClient)
        {
            try
            {
                Uri link = new Uri(_url);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressChanged);
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted);
                webClient.QueryString.Add("imageNumber", imageNumber.ToString());
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

        private void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled == false)
            {
                BitmapImage bmi = new BitmapImage();
                bmi.BeginInit();
                bmi.UriSource = new Uri(place);
                bmi.CacheOption = BitmapCacheOption.OnLoad;
                bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache | BitmapCreateOptions.DelayCreation;
                bmi.EndInit();

                WebClient obj = sender as WebClient;
                int imageNumber = Int32.Parse(obj.QueryString["imageNumber"]);
                switch (imageNumber)
                {
                    case 1:
                        FirstImage.Source = bmi;
                        Start1Btn.IsEnabled = true;
                        Stop1Btn.IsEnabled = false;
                        break;
                    case 2:
                        SecondImage.Source = bmi;
                        Start2Btn.IsEnabled = true;
                        Stop2Btn.IsEnabled = false;
                        break;
                    case 3:
                        ThirdImage.Source = bmi;
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
                Download(1, url, place, webClient1);
            }
        }

        private void Filling(TextBox tb)
        {
            if (tb.Text.Contains(" ") || tb.Text == "")
                tb.Text = "Введите URL";
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
                Download(2, url, place, webClient2);
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
                Download(3, url, place, webClient3);
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

