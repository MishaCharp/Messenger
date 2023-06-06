using Messenger.Entites;
using Messenger.Windows;
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

            var lastItem = DialogListBox.Items[DialogListBox.Items.Count - 1];

            DialogListBox.ScrollIntoView(lastItem);

            StickersPanel.ItemsSource = App.stickers;

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

        void SendSticker(int IdSticker)
        {
            App.Context.Messenger_Dialog.Where(x => x.Id == CurrentIdDialog).FirstOrDefault().LastMessage = "[Sticker]";
            App.Context.Messenger_Message.Add(new Entites.Messenger_Message
            {
                IdDialog = CurrentIdDialog,
                IdSticker = IdSticker,
                LastIdSenderUser = App.CurrentUser.Id,
                LastNicknameSenderUser = App.CurrentUser.Nickname,
                Time = DateTime.Now
            });
            App.Context.SaveChanges();

            MessageText.Text = "";

            var lastItem = DialogListBox.Items[DialogListBox.Items.Count - 1];

            DialogListBox.ScrollIntoView(lastItem);


            StickersBorder.Visibility = Visibility.Collapsed;

        }

        void SendMessage(string message)
        {
            App.Context.Messenger_Dialog.Where(x => x.Id == CurrentIdDialog).FirstOrDefault().LastMessage = message;
            App.Context.Messenger_Message.Add(new Entites.Messenger_Message
            {
                IdDialog = CurrentIdDialog,
                Message = message,
                LastIdSenderUser = App.CurrentUser.Id,
                LastNicknameSenderUser = App.CurrentUser.Nickname,
                Time = DateTime.Now
            });
            App.Context.SaveChanges();

            MessageText.Text = "";

            var lastItem = DialogListBox.Items[DialogListBox.Items.Count - 1];

            DialogListBox.ScrollIntoView(lastItem);

        }

        private void SendMessageButtonClick(object sender, RoutedEventArgs e)
        {

            var message = MessageText.Text;

            if (message != "")
            {
                SendMessage(message);
            }
            else
            {
                new NewMessageBox(App.Current.MainWindow, "Введите сообщение!").ShowDialog();
            }
        }

        private void IsEnterClicked(object sender, KeyEventArgs e)
        {

            var message = MessageText.Text;

            if (e.Key == Key.Enter)
            {

                if (message != "")
                {
                    SendMessage(message);
                }

            }
        }

        private void GiveStickersClick(object sender, RoutedEventArgs e)
        {
            StickersBorder.Visibility = StickersBorder.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void SendStickerClick(object sender, MouseButtonEventArgs e)
        {

            var sticker = (KeyValuePair<int, ImageSource>)((sender as Image).DataContext);

            SendSticker(sticker.Key);

        }
    }
}
