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

namespace Messenger.Windows
{
    /// <summary>
    /// Логика взаимодействия для CreateNewPost.xaml
    /// </summary>
    public partial class CreateNewPost : Window
    {
        public CreateNewPost()
        {
            InitializeComponent();

            var dateTime = DateTime.Now;

            TimeTB.Text = $"{dateTime.Hour}:{dateTime.Minute} {dateTime.Date.ToString("dd.MM.yyyy")}";

            AuthorTB.Text = App.CurrentUser.Nickname;
        }

        private void CloseBtn(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddPostClick(object sender, RoutedEventArgs e)
        {
            if (TitleTB.Text != "" && DescriptionTB.Text != "")
            {
                App.Context.Messenger_Post.Add(new Entites.Messenger_Post
                {
                    DateTime = DateTime.Now,
                    Title = TitleTB.Text,   
                    Description = DescriptionTB.Text,
                    IdUser = App.CurrentUser.Id

                });
                App.Context.SaveChanges();
                new Windows.NewMessageBox(Window.GetWindow(this), "Успех!", "Пост успешно добавлен!").ShowDialog();
                this.Close();
            }
            else
            {
                new Windows.NewMessageBox(Window.GetWindow(this), "Ошибка", "Заполните все поля!").ShowDialog();
            }
        }
    }
}
