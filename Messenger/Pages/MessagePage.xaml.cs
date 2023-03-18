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
using System.Windows.Threading;

namespace Messenger.Pages
{
    /// <summary>
    /// Логика взаимодействия для MessagePage.xaml
    /// </summary>
    public partial class MessagePage : Page
    {

        public List<Entites.Messenger_Dialog> dialogs { get; set; } = new List<Entites.Messenger_Dialog>();

        DispatcherTimer timer = new DispatcherTimer
        {
            Interval = new TimeSpan(0, 0, 0, 0, 500),
            IsEnabled = true
        };

        public MessagePage()
        {
            InitializeComponent();
            try
            {
                dialogs = App.Context.Messenger_Dialog.Where(x => x.IdFirstUser == App.CurrentUser.Id || x.IdSecondUser == App.CurrentUser.Id).ToList();
                MessageListBox.ItemsSource = dialogs;
                timer.Start();
                timer.Tick += TimerTick;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void TimerTick(object sender, EventArgs e)
        {
            try
            {
                dialogs = App.Context.Messenger_Dialog.Where(x => x.IdFirstUser == App.CurrentUser.Id || x.IdSecondUser == App.CurrentUser.Id).ToList();
                MessageListBox.ItemsSource = dialogs;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DialogMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                int IdDialog = ((sender as Border).DataContext as Entites.Messenger_Dialog).Id;

                NavigationService.Navigate(new DialogPage(IdDialog));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
