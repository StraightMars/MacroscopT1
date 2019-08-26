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
        UserControlViewModel vm1, vm2, vm3;
        public MainWindow()
        {
            Icon = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "cat.ico"));
            InitializeComponent();
            vm1 = new UserControlViewModel();
            vm2 = new UserControlViewModel();
            vm3 = new UserControlViewModel();
            var mainWindow = new MainWindowViewModel(vm1, vm2, vm3);
            DataContext = mainWindow;
            First.DataContext = vm1;
            Second.DataContext = vm2;
            Third.DataContext = vm3;
            string iniPlace = AppDomain.CurrentDomain.BaseDirectory + "фон.jpg";
        }
    }
}

