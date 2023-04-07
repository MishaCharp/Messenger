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

        DispatcherTimer timer = new DispatcherTimer{
            IsEnabled = true
        };

        public MessagePage()
        {
            InitializeComponent();
            App.AnimateBorderOpacity(this);
            try
            {
                dialogs = App.Context.Messenger_Dialog.Where(x => x.IdFirstUser == App.CurrentUser.Id || x.IdSecondUser == App.CurrentUser.Id).ToList();
                MessageListBox.ItemsSource = dialogs;
                timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
                timer.Start();
                timer.Tick += TimerTick;
            }
            catch (Exception ex)
            {
                new Windows.NewMessageBox(Window.GetWindow(this), "Ошибка", ex.Message).ShowDialog();
            }

        }

        public static Entites.user2_dbEntities1 _Context { get; set; } = null;

        int countDialogMessage = 0;

        int _countDialogMessage = 0;

        private void TimerTick(object sender, EventArgs e)
        {
            try
            {
                var dialogs = new Entites.user2_dbEntities1().Messenger_Dialog.Where(x => x.IdFirstUser == App.CurrentUser.Id || x.IdSecondUser == App.CurrentUser.Id).ToList();

                countDialogMessage = 0;

                foreach (var dialog in dialogs)
                {
                    countDialogMessage += dialog.Messenger_Message.Count;
                }

                if (countDialogMessage != _countDialogMessage)
                {
                    MessageListBox.ItemsSource = dialogs;
                    _countDialogMessage = countDialogMessage;
                }

            }
            catch (Exception ex)
            {
                new Windows.NewMessageBox(Window.GetWindow(this), "Ошибка", ex.Message).ShowDialog();
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
                new Windows.NewMessageBox(Window.GetWindow(this), "Ошибка", ex.Message).ShowDialog();
            }
        }
    }
}
