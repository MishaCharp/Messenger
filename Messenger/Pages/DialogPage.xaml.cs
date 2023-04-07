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
    /// Логика взаимодействия для DialogPage.xaml
    /// </summary>
    public partial class DialogPage : Page
    {

        public List<Entites.Messenger_Message> messages { get; set; } = new List<Entites.Messenger_Message>();

        DispatcherTimer timer = new DispatcherTimer
        {
            Interval = new TimeSpan(0, 0, 0, 0, 500),
            IsEnabled = true
        };

        int CurrentIdDialog = 0;

        public DialogPage(int IdDialog)
        {
            InitializeComponent();

            CurrentIdDialog = IdDialog;

            messages = App.Context.Messenger_Message.Where(x => x.IdDialog == CurrentIdDialog).ToList();
            DialogListBox.ItemsSource = messages;
            timer.Start();
            timer.Tick += TimerTick;
        }

        int messagesCount = 0;

        private void TimerTick(object sender, EventArgs e)
        {
            try
            {
                var messages = App.Context.Messenger_Message.Where(x => x.IdDialog == CurrentIdDialog).ToList();
                if (messages.Count != messagesCount)
                {
                    DialogListBox.ItemsSource = messages;
                    messagesCount = messages.Count;
                }
            }
            catch
            {

            }

        }

        private void SendMessageButtonClick(object sender, RoutedEventArgs e)
        {

            var message = MessageText.Text;

            if (message != "")
            {
                App.Context.Messenger_Dialog.Where(x=>x.Id ==  CurrentIdDialog).FirstOrDefault().LastMessage = message;
                App.Context.Messenger_Message.Add(new Entites.Messenger_Message
                {
                    IdDialog = CurrentIdDialog,
                    Message = message,
                    LastIdSenderUser = App.CurrentUser.Id,
                    LastNicknameSenderUser = App.CurrentUser.Nickname,
                    Time = DateTime.Now
                });
                App.Context.SaveChanges();
            }
        }
    }
}
