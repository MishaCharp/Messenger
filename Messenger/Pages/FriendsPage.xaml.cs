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

        public static List<Messenger_User> friends = null;

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

            friends = new List<Messenger_User>(App.Context.Messenger_User.Where(x => x.Id != App.CurrentUser.Id
            && !ids.Contains(x.Id) 
            && !idsTo.Contains(x.Id)
            && !idsFriend.Contains(x.Id)).OrderBy(x => x.Nickname));

            FriendsListBox.ItemsSource = friends;

            RequestsFromMeListBox.ItemsSource = App.Context.Messenger_User.Where(x => ids.Contains(x.Id)).ToList();

            RequestsToMeListBox.ItemsSource = App.Context.Messenger_User.Where(x => idsTo.Contains(x.Id)).ToList();

            FriendsListView.ItemsSource = App.Context.Messenger_User.Where(x=> idsFriend.Contains(x.Id)).ToList();

            if (IsPrepod.IsChecked != true)
            {
                if (SearchTextBox.Text != "")
                {
                    friends = friends.Where(x => x.Nickname.Contains(SearchTextBox.Text)).ToList();
                }
                if (SpecializationsCBox.SelectedIndex >= 0)
                {
                    var _spec = SpecializationsCBox.SelectedItem as Messenger_Specialization;
                    friends = friends.Where(x => x.Messenger_Band.IdSpecialization == _spec.Id).ToList();
                }
                if (GroupsCBox.SelectedIndex >= 0)
                {
                    var _group = GroupsCBox.SelectedItem as Messenger_Band;
                    friends = friends.Where(x => x.IdBand == _group.Id).ToList();
                }
                if(CountriesCBox.SelectedIndex >= 0)
                {
                    var _country = CountriesCBox.SelectedItem as Messenger_Country;
                    friends = friends.Where(x => x.Messenger_City.Messenger_Country.Id == _country.Id).ToList();
                }
                if(CitiesCBox.SelectedIndex >= 0)
                {
                    var _city = CitiesCBox.SelectedItem as Messenger_City;
                    friends = friends.Where(x => x.IdCity == _city.Id).ToList();
                }
            }
            else
            {
                friends = friends.Where(x=>x.IdRole == 2).ToList();
            }

            

            FriendsListBox.ItemsSource = friends;

            CountOfUserTextBlock.Text = $"Пользователи ({friends.Count})";

        }

        List<Messenger_Specialization> Specializations = App.Context.Messenger_Specialization.ToList();

        List<Messenger_Band> Bands = App.Context.Messenger_Band.ToList();

        List<Messenger_Country> Countries = App.Context.Messenger_Country.ToList();

        List<Messenger_City> Cities = App.Context.Messenger_City.ToList();

        public FriendsPage()
        {
            InitializeComponent();

            SpecializationsCBox.ItemsSource = Specializations;
            GroupsCBox.ItemsSource = Bands;
            CountriesCBox.ItemsSource = Countries;
            CitiesCBox.ItemsSource = Cities;

            App.AnimateBorderOpacity(this);

            RefreshData();
        }


        private void SearchTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshData();
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

        private void SpecializationChanged(object sender, SelectionChangedEventArgs e)
        {

            var _spec = (sender as ComboBox).SelectedItem as Messenger_Specialization;

            if (_spec != null)
            {
                GroupsCBox.ItemsSource = Bands.Where(x => x.IdSpecialization == _spec.Id);

            }

            RefreshData();

        }

        private void BandChanged(object sender, SelectionChangedEventArgs e)
        {

            var _band = (sender as ComboBox).SelectedItem as Messenger_Band;


            if (SpecializationsCBox.SelectedIndex < 0)
            {
                SpecializationsCBox.SelectedItem = Specializations.FirstOrDefault(x=>x.Id == _band.IdSpecialization);
            }

            RefreshData();
        }

        private void CountryChanged(object sender, SelectionChangedEventArgs e)
        {

            var _country = (sender as ComboBox).SelectedItem as Messenger_Country;

            if(_country!=null)
            {
                CitiesCBox.ItemsSource = Cities.Where(x => x.IdCountry == _country.Id);
            }

            RefreshData();

        }

        private void CityChanged(object sender, SelectionChangedEventArgs e)
        {
            var _city = (sender as ComboBox).SelectedItem as Messenger_City;


            if (CountriesCBox.SelectedIndex < 0)
            {
                CountriesCBox.SelectedItem = Countries.FirstOrDefault(x => x.Id == _city.IdCountry);
            }

            RefreshData();
        }

        private void CheckBoxChecked(object sender, RoutedEventArgs e)
        {
            a.IsEnabled = false;
            b.IsEnabled = false;
            c.IsEnabled = false;
            d.IsEnabled = false;

            RefreshData();

        }

        private void RefreshParams()
        {

            Specializations = App.Context.Messenger_Specialization.ToList();

            Bands = App.Context.Messenger_Band.ToList();

            Countries = App.Context.Messenger_Country.ToList();

            Cities = App.Context.Messenger_City.ToList();

            SpecializationsCBox.ItemsSource = Specializations;
            GroupsCBox.ItemsSource = Bands;
            CountriesCBox.ItemsSource = Countries;
            CitiesCBox.ItemsSource = Cities;
        }

        private void CheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            a.IsEnabled = !false;
            b.IsEnabled = !false;
            c.IsEnabled = !false;
            d.IsEnabled = !false;

            RefreshData();
        }

        private void ClearParamsBtn(object sender, RoutedEventArgs e)
        {
            GroupsCBox.SelectedIndex = -1;
            SpecializationsCBox.SelectedIndex = -1;
            CitiesCBox.SelectedIndex = -1;
            CountriesCBox.SelectedIndex = -1;

            RefreshParams();
        }
    }


}
