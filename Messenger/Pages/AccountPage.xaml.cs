using Messenger.Entites;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Messenger.Pages
{
    /// <summary>
    /// Логика взаимодействия для AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page
    {

        List<Entites.Messenger_City> cities = App.Context.Messenger_City.ToList();

        List<Entites.Messenger_Country> countryes = App.Context.Messenger_Country.ToList();

        void RefreshEditingData()
        {
            var user = App.CurrentUser;
            /////////////////////////////////////////////////////////////////////

            CountryesComboBox.ItemsSource = countryes;

            AvatarsComboBox.ItemsSource = App.avatars;

            CityComboBox.ItemsSource = cities;

            NicknameBox.Text = user.Nickname;
            LoginBox.Text = user.Login;
            PasswordBox.Password = user.Password;
            DescriptionBox.Text = user.Description;
            PhoneBox.Text = user.Phone;
            CountryesComboBox.SelectedItem = countryes.FirstOrDefault(x => x.Id == user.Messenger_City.Messenger_Country.Id);
            CityComboBox.SelectedItem = cities.FirstOrDefault(x => x.Id == user.IdCity);
            AvatarsComboBox.SelectedIndex = App.avatars.Where(x => x.Key == user.IdAvatar).First().Key-1;
            /////////////////////////////////////////////////////////////////////

            RoundedAvatarImage.Source = user.AvatarImageSource;

            CountryTextBlock.Text = user.Messenger_City.Messenger_Country.Country + ", " + user.Messenger_City.City;

            PhoneTextBlock.Text = user.Phone;

            DescriptionTextBlock.Text = user.Description;

            LoginTextBlock.Text = user.Nickname;
        }

        public AccountPage()
        {
            InitializeComponent();

            RefreshEditingData();

            App.AnimateBorderOpacity(AccountGrid);


        }

        private void OpenAccountButtonClick(object sender, RoutedEventArgs e)
        {
            AccountGrid.Visibility = Visibility.Hidden;
            EditAccountGrid.Visibility = Visibility.Visible;
            RefreshEditingData();
            App.AnimateBorderOpacity(EditAccountGrid);
        }

        private void CloseEditorTextMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AccountGrid.Visibility = Visibility.Visible;
            EditAccountGrid.Visibility = Visibility.Hidden;
            App.AnimateBorderOpacity(AccountGrid);
        }

        private void EditAccountButtonClick(object sender, RoutedEventArgs e)
        {


            if (NicknameBox.Text != "")
            {

                if (LoginBox.Text != "" || PasswordBox.Password != "")
                {

                    if (CountryesComboBox.SelectedIndex >= 0 || CityComboBox.SelectedIndex >= 0)
                    {

                        if (PhoneBox.Text != "")
                        {

                            if (AvatarsComboBox.SelectedIndex >= 0)
                            {

                                if (DescriptionBox.Text != "")
                                {

                                    App.CurrentUser.Nickname = NicknameBox.Text;
                                    App.CurrentUser.Login = LoginBox.Text;
                                    App.CurrentUser.Password = PasswordBox.Password;
                                    App.CurrentUser.Description = DescriptionBox.Text;
                                    App.CurrentUser.Phone = PhoneBox.Text;
                                    App.CurrentUser.IdAvatar = ((KeyValuePair<int, ImageSource>)AvatarsComboBox.SelectedItem).Key;
                                    App.CurrentUser.IdCity = (CityComboBox.SelectedItem as Messenger_City).Id;

                                    App.Context.SaveChanges();

                                    new Windows.NewMessageBox(Window.GetWindow(this), "Успешно!", $"Данные обновлены!").ShowDialog();

                                    AccountGrid.Visibility = Visibility.Visible;
                                    EditAccountGrid.Visibility = Visibility.Hidden;
                                    RefreshEditingData();
                                    App.AnimateBorderOpacity(AccountGrid);

                                }
                                else
                                {
                                    new Windows.NewMessageBox(Window.GetWindow(this), "Ошибка", "Введите описание страницы!").ShowDialog();
                                }

                            }
                            else
                            {
                                new Windows.NewMessageBox(Window.GetWindow(this), "Ошибка", "Выберите аватарку!").ShowDialog();
                            }

                        }
                        else
                        {
                            new Windows.NewMessageBox(Window.GetWindow(this), "Ошибка", "Заполните поле [телефон]!").ShowDialog();
                        }

                    }
                    else
                    {
                        new Windows.NewMessageBox(Window.GetWindow(this), "Ошибка", "Выберите город и страну!").ShowDialog();
                    }

                }
                else
                {
                    new Windows.NewMessageBox(Window.GetWindow(this), "Ошибка", "Заполните поля логин и пароль!").ShowDialog();
                }
            }
            else
            {
                new Windows.NewMessageBox(Window.GetWindow(this), "Ошибка", "Заполните поле [Никнейм]!").ShowDialog();
            }


        }


        private void NicknameBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (NicknameBox.Text.Length > 0)
            {
                PlaceholderNickname.Visibility = Visibility.Hidden;
            }
            else
            {
                PlaceholderNickname.Visibility = Visibility.Visible;
            }
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

        private void PhoneBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (PhoneBox.Text.Length > 0)
            {
                PlaceholderPhone.Visibility = Visibility.Hidden;
            }
            else
            {
                PlaceholderPhone.Visibility = Visibility.Visible;
            }
        }

        private void DescriptionBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (DescriptionBox.Text.Length > 0)
            {
                PlaceholderDesciption.Visibility = Visibility.Hidden;
            }
            else
            {
                PlaceholderDesciption.Visibility = Visibility.Visible;
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

        private void CountryesComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var box = (ComboBox)sender;
            if (box.SelectedItem != null)
            {
                var selectedAvatar = (Messenger_Country)box.SelectedItem;

                CountryTextBlock.Text = selectedAvatar.Country;

                CountryPanel.Visibility = Visibility.Visible;

                PlaceholderAvatar.Visibility = Visibility.Visible;

                if ((CityComboBox.SelectedItem as Messenger_City) != null)
                {
                    if ((CityComboBox.SelectedItem as Messenger_City).IdCountry != selectedAvatar.Id)
                    {
                        CityComboBox.SelectedIndex = 0;

                        CityComboBox.SelectedValue = "Город";

                        CityComboBox.Text = "Город";
                    }
                }


                CityComboBox.ItemsSource = cities.Where(x => x.IdCountry == selectedAvatar.Id).ToList();
            }
            else
            {
                CountryPanel.Visibility = Visibility.Hidden;
            }
        }

        private void CityComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCity = (ComboBox)sender;

            var City = selectedCity.SelectedItem as Messenger_City;

            if (selectedCity.SelectedItem != null)
            {

                if (CountryesComboBox.SelectedItem == null)
                {
                    CountryesComboBox.SelectedItem = countryes.FirstOrDefault(x => x.Id == City.IdCountry);
                }

            }
        }

        private void AvatarsComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var box = (ComboBox)sender;
            if (box.SelectedItem != null)
            {
                var selectedAvatar = (KeyValuePair<int, ImageSource>)box.SelectedItem;
                PlaceholderAvatar.Text = $"Выбрана аватарка \"{selectedAvatar.Key}\"";

                RoundedAvatarImage.Source = selectedAvatar.Value;

                RoundedAvatar.Visibility = Visibility.Visible;

                PlaceholderAvatar.Visibility = Visibility.Visible;

            }
            else
            {
                PlaceholderAvatar.Visibility = Visibility.Hidden;

            }
        }
    }
}
