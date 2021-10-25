using meria.MVVM.ViewModel;
using meria.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;

namespace meria.MVVM.View
{
    public partial class MainPage : Window
    {
        public delegate void DeleteAll();
        public static event DeleteAll deleteAllEvent;
        private double CurrentActualHeight=0;
        private double SettingsActualHeight = 0;
        public MainPage()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            BaseLogic.SetImage += (x) => 
            {
                if(x)
                    PauseBtn.Source = new BitmapImage(new Uri(@"/Images/pause_26px.png", UriKind.Relative));
                else
                    PauseBtn.Source = new BitmapImage(new Uri(@"/Images/play_26px.png", UriKind.Relative));
            };
            DataBaseOnly d = new DataBaseOnly();
            d.del += (x) => 
            {
                string xxx1= (Convert.ToInt32(x.Rows[0][0].ToString()) / 60).ToString();
                string xxx2 = (Convert.ToInt32(x.Rows[0][0].ToString()) % 60).ToString();

                if (xxx1.Length == 1)
                    xxx1 = "0" + xxx1;
                if (xxx2.Length == 1)
                    xxx2 = "0" + xxx2;
                minute.Text = xxx1;
                second.Text = xxx2;
            };
            d.SoursData("SELECT `value` FROM `gorkenesh`.`options` WHERE `KEY`='defaultTime' LIMIT 1");
        }
        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)=>DragMove();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (current.ActualHeight < 50) 
            {
                current.Height = new GridLength(CurrentActualHeight);//заменить
                currentGridImage.Source= new BitmapImage(new Uri(@"/Images/subtract.png", UriKind.Relative));
            }
            else 
            {
                currentGridImage.Source = new BitmapImage(new Uri(@"/Images/restore.png", UriKind.Relative));
                if(CurrentActualHeight==0)
                    CurrentActualHeight = current.ActualHeight;
                if (SettingsActualHeight == 0)
                    SettingsActualHeight = settings.ActualHeight;
                current.Height = new GridLength(45);//заменить
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)=>Close();
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
                down_up_btn.Source = new BitmapImage(new Uri(@"/Images/base/restore_down.png", UriKind.Relative));
            }
            else 
            {
                WindowState = WindowState.Normal;
                down_up_btn.Source = new BitmapImage(new Uri(@"/Images/base/unchecked_checkbox.png", UriKind.Relative));
            }
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (settings.ActualHeight < 50)
            {
                settings.Height = new GridLength(SettingsActualHeight);//заменить
                settingsGridImage.Source = new BitmapImage(new Uri(@"/Images/subtract.png", UriKind.Relative));
            }
            else
            {
                settingsGridImage.Source = new BitmapImage(new Uri(@"/Images/restore.png", UriKind.Relative));
                if (SettingsActualHeight == 0)
                    SettingsActualHeight = settings.ActualHeight;
                if (CurrentActualHeight == 0)
                    CurrentActualHeight = current.ActualHeight;
                settings.Height = new GridLength(45);//заменить
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        public delegate void FuckAll();
        public static event FuckAll destructor;
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            destructor();
        }
        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            int o = Convert.ToInt32((sender as Button).Uid);
            if (o == 1 || o == -1)
            {
                try
                {
                    if (Convert.ToInt32(minute.Text) + o < 0)
                        return;
                    minute.Text = (Convert.ToInt32(minute.Text) + o).ToString();
                }
                catch
                {
                    minute.Text = "00";
                }
            }
            else
            {
                try
                {
                    if (Convert.ToInt32(second.Text) + o < 0)
                        return;
                    if (o + Convert.ToInt32(second.Text) > 59)
                    {
                        minute.Text = (Convert.ToInt32(minute.Text) + o).ToString();
                        second.Text = (Convert.ToInt32(second.Text) + o - 60).ToString();
                        return;
                    }
                    second.Text = (Convert.ToInt32(second.Text) + o).ToString();
                }
                catch
                {
                    second.Text = "00";
                }
            }
        }
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            try
            {
                DataBaseOnly d = new();
                d.InsertData("UPDATE `gorkenesh`.`options` SET `value`='" + (Convert.ToInt32(minute.Text) * 60 + Convert.ToInt32(second.Text)) + "' WHERE  `key`='defaultTime'");
            }
            finally 
            {
                minute.Text = "00";
                second.Text = "00";
            }
        }
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            DataBaseOnly d = new ();
            d.InsertData("DELETE FROM `gorkenesh`.`queue` WHERE  `id`>0");
            deleteAllEvent();
        }
        bool flag = false;
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Tab"|| e.Key.ToString() == "LeftCtrl") 
            {
                if (e.Key.ToString() == "LeftCtrl" && !flag)
                    flag = true;
                if (e.Key.ToString() == "Tab" && flag) 
                {
                        flag = false;
                        TabCont.SelectedIndex = (int)TabCont.SelectedIndex == 0 ? 1 : 0;
                }
            }
            else 
            {
                flag = false;
            }
            e.Handled = true;
        }
    }
}
