using Messenger.Entites;
using Messenger.Windows;
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

        public static ObservableCollection<Messenger_User> friends = null;

        void RefreshData()
        {

            var requests = App.Context.Messenger_RequestOfFriendship.Where(x=>x.SendeUserrId==App.CurrentUser.Id).ToList();

            var _requests = App.Context.Messenger_RequestOfFriendship.Where(x=>x.RecieverUserId==App.CurrentUser.Id).ToList();

            List<int> ids = new List<int>();

            List<int> idsTo = new List<int>();

            List<int> idsFriend = new List<int>();

            foreach (var request in requests)
            {
                ids.Add(request.RecieverUserId);
            }

            foreach(var _request in _requests)
            {
                idsTo.Add(_request.SendeUserrId);
            }

            foreach(var friend in App.Context.Messenger_Friendship.Where(x => x.IdFirstUser == App.CurrentUser.Id || x.IdSecondUser==App.CurrentUser.Id).ToList())
            {
                if (friend.IdFirstUser == App.CurrentUser.Id)
                {
                    idsFriend.Add(friend.IdSecondUser);
                }
                else
                {
                    idsFriend.Add(friend.IdFirstUser);
                }
            }

            friends = new ObservableCollection<Messenger_User>(App.Context.Messenger_User.Where(x => x.Id != App.CurrentUser.Id
            && !ids.Contains(x.Id) 
            && !idsTo.Contains(x.Id)
            && !idsFriend.Contains(x.Id)).OrderBy(x => x.Nickname));

            CountOfUserTextBlock.Text = $"Пользователи ({friends.Count})";

            FriendsListBox.ItemsSource = friends;

            RequestsFromMeListBox.ItemsSource = App.Context.Messenger_User.Where(x => ids.Contains(x.Id)).ToList();

            RequestsToMeListBox.ItemsSource = App.Context.Messenger_User.Where(x => idsTo.Contains(x.Id)).ToList();

            FriendsListView.ItemsSource = App.Context.Messenger_User.Where(x=> idsFriend.Contains(x.Id)).ToList();

        }

        public FriendsPage()
        {
            InitializeComponent();

            App.AnimateBorderOpacity(this);

            RefreshData();
        }


        private void SearchTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            FriendsListBox.ItemsSource = friends.Where(x => x.Nickname.Contains(SearchTextBox.Text));
        }

        private void AddFriendButtonClick(object sender, RoutedEventArgs e)
        {

            var friend = (sender as Button).DataContext as Messenger_User;

            if (friend != null)
            {
                try
                {
                    App.Context.Messenger_RequestOfFriendship.Add(new Messenger_RequestOfFriendship
                    {
                        SendeUserrId = App.CurrentUser.Id,
                        RecieverUserId = friend.Id,
                    });
                    App.Context.SaveChanges();

                    RefreshData();
                }
                catch (Exception ex)
                {
                    new Windows.NewMessageBox(Window.GetWindow(this), "Ошибка", ex.Message).ShowDialog();
                }

            }
            else
            {
                new Windows.NewMessageBox(Window.GetWindow(this), "Ошибка", "Пользователь = null").ShowDialog();
            }

        }

        private void AgreeAddFriendButtonClick(object sender, RoutedEventArgs e)
        {

            var SelectedUser = ((sender as Button).DataContext as Messenger_User).Id;

            var request = App.Context.Messenger_RequestOfFriendship
                .Where(x => x.SendeUserrId == SelectedUser && x.RecieverUserId == App.CurrentUser.Id)
                .FirstOrDefault();

            if (request != null)
            {
                try
                {
                    App.Context.Messenger_Friendship.Add(new Messenger_Friendship
                    {
                        IdFirstUser = App.CurrentUser.Id,
                        IdSecondUser = request.SendeUserrId,
                        DateTimeOfFriendship = DateTime.Now,
                    });
                    App.Context.Messenger_RequestOfFriendship.Remove(request);
                    App.Context.SaveChanges();

                    RefreshData();
                }
                catch (Exception ex)
                {
                    new Windows.NewMessageBox(Window.GetWindow(this), "Ошибка", ex.Message).ShowDialog();
                }

            }
            else
            {
                new Windows.NewMessageBox(Window.GetWindow(this), "Ошибка", "Запрос на дружбу = null").ShowDialog();
            }
        }

        private void DisagreeAddFriendButtonClick(object sender, RoutedEventArgs e)
        {
            var SelectedUser = ((sender as Button).DataContext as Messenger_User).Id;

            var request = App.Context.Messenger_RequestOfFriendship
                .Where(x => x.SendeUserrId == SelectedUser && x.RecieverUserId == App.CurrentUser.Id)
                .FirstOrDefault();

            if (request != null)
            {
                try
                {
                    App.Context.Messenger_RequestOfFriendship.Remove(request);
                    App.Context.SaveChanges();

                    RefreshData();
                }
                catch (Exception ex)
                {
                    new Windows.NewMessageBox(Window.GetWindow(this), "Ошибка", ex.Message).ShowDialog();
                }

            }
            else
            {
                new Windows.NewMessageBox(Window.GetWindow(this), "Ошибка", "Запрос на дружбу = null").ShowDialog();
            }
        }

        private void UndoRequestButtonClick(object sender, RoutedEventArgs e)
        {

            var SelectedUser = ((sender as Button).DataContext as Messenger_User).Id;

            var request = App.Context.Messenger_RequestOfFriendship
                .Where(x => x.RecieverUserId == SelectedUser && x.SendeUserrId == App.CurrentUser.Id)
                .FirstOrDefault();

            if (request != null)
            {
                try
                {
                    App.Context.Messenger_RequestOfFriendship.Remove(request);
                    App.Context.SaveChanges();

                    RefreshData();
                }
                catch (Exception ex)
                {
                    new Windows.NewMessageBox(Window.GetWindow(this), "Ошибка", ex.Message).ShowDialog();
                }

            }
            else
            {
                new Windows.NewMessageBox(Window.GetWindow(this), "Ошибка", "Запрос на дружбу = null").ShowDialog();
            }
        }

        private void BorderMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if(MyFriendsGrid.Visibility == Visibility.Visible)
            {
                MyFriendsGrid.Visibility = Visibility.Hidden;
                SearchFriendsGrid.Visibility = Visibility.Visible;
                App.AnimateBorderOpacity(MyFriendsGrid);
                ButtonText.Text = "Список друзей";
            }
            else
            {
                MyFriendsGrid.Visibility = Visibility.Visible;
                SearchFriendsGrid.Visibility = Visibility.Hidden;
                App.AnimateBorderOpacity(SearchFriendsGrid);
                ButtonText.Text = "Поиск друзей";
            }

        }

        private void WriteMessageTextMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            var user = (sender as TextBlock).DataContext as Messenger_User;

            new WriteWindow(Window.GetWindow(this), user).ShowDialog();
        }
    }


}
