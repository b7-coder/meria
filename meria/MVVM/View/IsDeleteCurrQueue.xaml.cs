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
using System.Windows.Shapes;

namespace meria.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для IsDeleteCurrQueue.xaml
    /// </summary>
    public partial class IsDeleteCurrQueue : Window
    {
        public IsDeleteCurrQueue(string x)
        {
            InitializeComponent();
            UserName.Text += "\n"+x;
        }
        public delegate void GetInfo();
        public event GetInfo returnOk;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            returnOk();
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
