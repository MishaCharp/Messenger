using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Messenger.Pages
{
    /// <summary>
    /// Логика взаимодействия для MessengerPage.xaml
    /// </summary>
    public partial class MessengerPage : Page
    {
        public MessengerPage()
        {
            InitializeComponent();
            OpenMenuTextBlock.Text = "<<";
            AccountPanel.Background = new SolidColorBrush(Color.FromRgb(4, 18, 32));

            MessengerFrame.Navigate(new AccountPage());
        }

        void SelectIcon(string NameIcon)
        {

            Page page = null;

            foreach(var icon in LeftMenu.Children)
            {
                if(NameIcon==(icon as DockPanel).Name)
                {
                    (icon as DockPanel).Background = new SolidColorBrush(Color.FromRgb(4,18,32));
                    App.AnimateBorderOpacityFast(icon as DockPanel);
                    switch (NameIcon)
                    {
                        case "AccountPanel":
                            page = new AccountPage();
                            break;
                        case "NewsPanel":
                            page = new NewsPage();
                            break;
                        case "MessagePanel":
                            page = new MessagePage();
                            break;
                        case "FriendsPanel":
                            page = new FriendsPage();
                            break;
                        case "SavedPanel":
                            page = new SavedPage();
                            break;
                        case "PowerOff":
                            Application.Current.Shutdown();
                            break;
                    }

                    try
                    {
                        MessengerFrame.NavigationService.RemoveBackEntry();
                        MessengerFrame.Content = null;
                        MessengerFrame.NavigationService.RemoveBackEntry();
                        GC.Collect();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    

                    MessengerFrame.Navigate(page);
                }
                else
                {
                    (icon as DockPanel).Background = Brushes.Transparent;
                }
            }

        }

        private void IconMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectIcon((sender as DockPanel).Name);
        }

        int Counter = 0;


        private async void TextBlockMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ColumnDefinition columnDefinition = MenuColumn;// получить ссылку на ColumnDefinition, который нужно уменьшить
            double targetWidth = 0;// ширина, до которой нужно уменьшить ColumnDefinition
            double step = (columnDefinition.Width.Value - targetWidth) / 10; // количество пикселей, на которые нужно уменьшить ColumnDefinition на каждой итерации
            if (Counter % 2 == 0)
            {
                targetWidth = 70;
                while (columnDefinition.Width.Value > targetWidth)
                {
                    columnDefinition.Width = new GridLength(columnDefinition.Width.Value - step);
                    await Task.Delay(10); // небольшая задержка между итерациями
                }
                Counter++;
                OpenMenuTextBlock.Text = ">>";
            }
            else
            {
                targetWidth = 170;
                while (columnDefinition.Width.Value < targetWidth)
                {
                    columnDefinition.Width = new GridLength(columnDefinition.Width.Value + step);
                    await Task.Delay(10); // небольшая задержка между итерациями
                }
                Counter++;
                OpenMenuTextBlock.Text = "<<";
            }

        }
    }
}
