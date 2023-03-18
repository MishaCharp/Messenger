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

namespace Messenger.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {

        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (LoginBox.Text.Length > 0)
            {
                PlaceholderLogin.Visibility = Visibility.Hidden;
            }
            else
            {
                PlaceholderLogin.Visibility = Visibility.Visible;
            }
        }

        private void PasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password.Length > 0)
            {
                PlaceholderPassword.Visibility = Visibility.Hidden;
            }
            else
            {
                PlaceholderPassword.Visibility = Visibility.Visible;
            }
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            if(LoginBox.Text!="" && PasswordBox.Password!="")
            {
                var user = App.Context.Messenger_User.Where(x => x.Login == LoginBox.Text && x.Password == PasswordBox.Password).FirstOrDefault();
                if (user != null)
                {
                    App.CurrentUser = user;
                    NavigationService.Navigate(new MessengerPage());
                }
                else
                {
                    new Windows.NewMessageBox(Window.GetWindow(this), "Ошибка", "Указанный пользователь не найден!").ShowDialog();
                }
                
            }
            else
            {
                new Windows.NewMessageBox(Window.GetWindow(this),"Ошибка","Все поля должны быть заполнены!").ShowDialog();
            }
        }
    }
}
