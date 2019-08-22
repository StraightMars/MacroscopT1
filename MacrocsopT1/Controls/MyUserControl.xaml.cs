using System;
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

namespace MacrocsopT1.Controls
{
    /// <summary>
    /// Логика взаимодействия для MyUserControl.xaml
    /// </summary>
    public partial class MyUserControl : UserControl
    {
        public MyUserControl()
        {
            InitializeComponent();
            DataContext = new UserControlViewModel();
            ImageTb.Text = "Enter URL...";
            //Binding binding = new Binding();
            //binding.ElementName = "ImageTB";
            //binding.Path = new PropertyPath("Text");
            //ImageTb.SetBinding(TextBox.TextProperty, binding);
        }

        private void ImageTb_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ImageTb.Clear();
        }

        private void ImageTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ImageTb.Text == "" || ImageTb.Text == "Enter URL...")
                ImageTb.Text = "Enter URL...";
        }
    }
}
