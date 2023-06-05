using Messenger.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

namespace Messenger.Windows
{
    /// <summary>
    /// Логика взаимодействия для WriteWindow.xaml
    /// </summary>
    public partial class WriteWindow : Window
    {

        Messenger_User Reciever = new Messenger_User();

        public WriteWindow(Window Parent, Messenger_User _Reciever)
        {
            InitializeComponent();
            this.Owner = Parent;
            Reciever = _Reciever;
            NicknameTextBlock.Text = Reciever.Nickname;
        }

        private void SendMessageButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = App.Context.Messenger_Dialog.Where(x=> (x.IdFirstUser==App.CurrentUser.Id && x.IdSecondUser==Reciever.Id) || (x.IdFirstUser == Reciever.Id && x.IdSecondUser == App.CurrentUser.Id)).FirstOrDefault();

            if (MessageTextBox.Text != "")
            {
                if (dialog != null)
                {
                    App.Context.Messenger_Message.Add(new Messenger_Message
                    {
                        Message = MessageTextBox.Text,
                        IdDialog = dialog.Id,
                        Time = DateTime.Now,
                        LastIdSenderUser = App.CurrentUser.Id,
                        LastNicknameSenderUser = App.CurrentUser.Nickname,
                    });
                    dialog.LastMessage = MessageTextBox.Text;
                    App.Context.SaveChanges();
                    this.Close();
                }
                else
                {
                    Messenger_Dialog newDialog = new Messenger_Dialog()
                    {
                        IdFirstUser = App.CurrentUser.Id,
                        IdSecondUser = Reciever.Id,
                        LastMessage = ""
                    };

                    App.Context.Messenger_Dialog.Add(newDialog);

                    App.Context.SaveChanges();

                    App.Context.Messenger_Message.Add(new Messenger_Message
                    {
                        Message = MessageTextBox.Text,
                        IdDialog = newDialog.Id,
                        Time = DateTime.Now,
                        LastIdSenderUser = App.CurrentUser.Id,
                        LastNicknameSenderUser = App.CurrentUser.Nickname,
                    });

                    newDialog.LastMessage = MessageTextBox.Text;

                    App.Context.SaveChanges();

                    new Windows.NewMessageBox(Window.GetWindow(this), "Успех", "Сообщение отправлено!").ShowDialog();

                    this.Close();
                }
            }
            else
            {
                new Windows.NewMessageBox(Window.GetWindow(this), "Ошибка", "Сообщение не может быть пустым!").ShowDialog();
            }

        }
    }
}
