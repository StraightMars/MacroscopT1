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
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private UserControlViewModel first;
        private UserControlViewModel second;
        private UserControlViewModel third;
        private double percent;
        public double Percent
        {
            get { return percent; }
            set
            {
                percent = value;
                OnPropertyChanged();
            }
        }

        public ICommand StartAllCommand { get; }
        public void StartAll(object o)
        {
            IsEnabledStartAll = false;
            Percent = 0;
            first.Start("first");
            second.Start("second");
            third.Start("third");

            GetPercentage();
            IsEnabledStartAll = true;

        }

        private bool isEnabledStartAll;
        public bool IsEnabledStartAll
        {
            get { return isEnabledStartAll; }
            set
            {
                isEnabledStartAll = value;
                OnPropertyChanged();
            }
        }

        private void GetPercentage()
        {
            Task.Run(() =>
            {
                while (first.percentage != 100 && second.percentage != 100 && third.percentage != 100 && Percent != 100)
                {
                    Percent = (first.percentage + second.percentage + third.percentage) / 3;
                }
            });

            Task.Run(() =>
            {
                while (Percent < 100)
                {
                    Percent = (first.percentage + second.percentage + third.percentage) / 3;
                }
            });
        }

        public MainWindowViewModel(UserControlViewModel vm1, UserControlViewModel vm2, UserControlViewModel vm3)
        {
            first = vm1;
            second = vm2;
            third = vm3;
            IsEnabledStartAll = true;
            StartAllCommand = new RelayCommand(StartAll);
            GetPercentage();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
