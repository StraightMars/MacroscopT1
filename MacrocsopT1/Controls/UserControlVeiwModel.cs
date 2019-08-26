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
        private BitmapImage bmi;

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

        public double percentage;
        public double Percentage
        {
            get { return percentage; }
            set
            {
                percentage = value;
                OnPropertyChanged();
            }
        }

        private string place;

        private bool isEnabledStart;
        public bool IsEnabledStart
        {
            get { return isEnabledStart; }
            set
            {
                isEnabledStart = value;
                OnPropertyChanged();
            }
        }

        private bool isEnabledStop;
        public bool IsEnabledStop
        {
            get { return isEnabledStop; }
            set
            {
                isEnabledStop = value;
                OnPropertyChanged();
            }
        }
        public UserControlViewModel()
        {
            StartCommand = new RelayCommand(Start);
            StopCommand = new RelayCommand(Stop);
            IsEnabledStart = true;
            IsEnabledStop = false;
        }

        //Random rnd;
        //private Guid GetRandomGuid()
        //{
        //    rnd = new Random();
        //    Guid g = new Guid("0000000000000000000000000000" + rnd.Next(1000, 10000).ToString());
        //    return g;
        //}

        public ICommand StartCommand { get; }
        public void Start(object o)
        {
            IsEnabledStart = false;
            IsEnabledStop = true;
            place = AppDomain.CurrentDomain.BaseDirectory + o + ".jpg";
            webClient = new WebClient();
            Download(webClient);
        }

        public ICommand StopCommand { get; }
        public void Stop(object o)
        {
            webClient.CancelAsync();
            IsEnabledStart = true;
            IsEnabledStop = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void Download(WebClient webClient)
        {
            try
            {
                Uri link = new Uri(url);

                webClient.DownloadFileAsync(link, place);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressChanged);
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted);
            }
            catch (Exception)
            {
                MessageBox.Show("Wrong URL...", "Wait, what..?", MessageBoxButton.OK, MessageBoxImage.Error);
                IsEnabledStart = true;
                IsEnabledStop = false;
            }
        }

        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Percentage = double.Parse(e.BytesReceived.ToString()) / double.Parse(e.TotalBytesToReceive.ToString()) * 100;
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
                IsEnabledStart = true;
                IsEnabledStop = false;
            }
        }
    }
}
