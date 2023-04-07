using Messenger.Entites;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
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

    public partial class FriendsPage : Page
    {

        public static ObservableCollection<Messenger_User> friends = new ObservableCollection<Messenger_User>(App.Context.Messenger_User.Where(x=>x.Id!=App.CurrentUser.Id).OrderBy(x=>x.Nickname).ToList());

        public FriendsPage()
        {
            InitializeComponent();

            App.AnimateBorderOpacity(this);

            FriendsListBox.ItemsSource = friends;

            RequestsFromMeListBox.ItemsSource = friends;

            RequestsToMeListBox.ItemsSource = friends;
        }


        private void SearchTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            FriendsListBox.ItemsSource = friends.Where(x=>x.Nickname.Contains(SearchTextBox.Text));
        }

        private void AddFriendButtonClick(object sender, RoutedEventArgs e)
        {

            var friend = (sender as Button).DataContext as Messenger_User;

            if(friend != null) 
            {
                //Не забыть обновить БД и свойства скопировать
                //создаем заявку и меня текст кнопки на "заявка отправлена"
            }
            else
            {
                //выдаем ошибку
            }

        }
    }


}
