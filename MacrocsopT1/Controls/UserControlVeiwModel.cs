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
using System.Drawing;
using System.Runtime.CompilerServices;


namespace MacrocsopT1
{
    class UserControlViewModel : INotifyPropertyChanged
    {
        private string place;
        private BitmapImage bmi;

        private RelayCommand startCommand, stopCommand;

        private double bytesIn = 0, percentage;
        private double totalB = 0, prevBytes = 0;

        private WebClient webClient;

        private string url;
        public string Url
        {
            get { return url; }
            set
            {
                url = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage image;
        public BitmapImage Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand StartCommand
        {
            get
            {
                return startCommand ??
                    (startCommand = new RelayCommand(obj =>
                    {
                        place = AppDomain.CurrentDomain.BaseDirectory + new Guid().ToString() + ".jpg";
                        webClient = new WebClient();
                        Download(webClient);
                    }));
            }
        }
        public RelayCommand StopCommand
        {
            get
            {
                return stopCommand ??
                    (stopCommand = new RelayCommand(obj =>
                    {
                        webClient.CancelAsync();
                    }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private async Task CheckFileSize(WebClient wc, Uri source)
        {
            Stream stream = await wc.OpenReadTaskAsync(source);
            double size = double.Parse(wc.ResponseHeaders["Content-Length"]);
            totalB += size;
            stream.Close();
        }

        private async void Download(WebClient webClient)
        {
            try
            {
                Uri link = new Uri(url);

                await CheckFileSize(webClient, link);

                webClient.DownloadFileAsync(link, place);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressChanged);
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted);
            }
            catch (Exception)
            {
                MessageBox.Show("Wrong URL...", "Wait, what..?", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesInTmp = bytesIn;

            if (bytesInTmp < e.TotalBytesToReceive)
            {
                if (e.BytesReceived >= prevBytes)
                {
                    bytesIn = e.BytesReceived;
                    prevBytes = e.BytesReceived;
                }
            }
            else
            {
                if (e.BytesReceived != e.TotalBytesToReceive)
                {
                    if (e.BytesReceived >= prevBytes)
                    {
                        bytesIn = bytesInTmp + (e.BytesReceived - prevBytes);
                        prevBytes = e.BytesReceived;
                    }
                }
                else
                {
                    bytesIn = bytesInTmp + (e.BytesReceived - prevBytes);
                    prevBytes = 0;

                }
            }
            percentage = bytesIn / totalB * 100;
        }

        private void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled == false)
            {
                bmi = new BitmapImage();
                bmi.BeginInit();
                bmi.CacheOption = BitmapCacheOption.OnLoad;
                bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bmi.UriSource = new Uri(place);
                bmi.EndInit();

                Image = bmi;
            }
        }
    }
}
