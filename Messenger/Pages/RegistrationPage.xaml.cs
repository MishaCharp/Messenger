using Messenger.Entites;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using System.Threading;
using System.Windows.Media.Animation;
using System.Diagnostics.Eventing.Reader;
using Messenger.Windows;
using System.ComponentModel;

namespace Messenger.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {

        public string base64Image = "";

        List<Entites.Messenger_City> cities = App.Context.Messenger_City.ToList();

        List<Entites.Messenger_Country> countryes = App.Context.Messenger_Country.ToList();

        public RegistrationPage()
        {
            InitializeComponent();

            CountryesComboBox.ItemsSource = countryes;

            AvatarsComboBox.ItemsSource = App.avatars;

            CityComboBox.ItemsSource = cities;

            RoundedAvatar.Visibility = Visibility.Hidden;

            CountryPanel.Visibility = Visibility.Hidden;

            PhonePanel.Visibility = Visibility.Hidden;

            DescriptionPanel.Visibility = Visibility.Hidden;
        }

        void ChangeProgressBar(double AddValue)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = ProgressBarAccount.Value;
            animation.To = AddValue;
            animation.Duration = TimeSpan.FromSeconds(0.2);
            ProgressBarAccount.BeginAnimation(ProgressBar.ValueProperty, animation);
        }

        private void SignUpButtonClick(object sender, RoutedEventArgs e)
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

                                    App.Context.Messenger_User.Add(new Messenger_User
                                    {
                                        Nickname = NicknameBox.Text,
                                        Login = LoginBox.Text,
                                        Password = PasswordBox.Password,
                                        Phone = PhoneBox.Text,
                                        IdAvatar = ((KeyValuePair<int, ImageSource>)AvatarsComboBox.SelectedItem).Key,
                                        IdCity = (CityComboBox.SelectedItem as Messenger_City).Id,
                                        Description = DescriptionBox.Text

                                    });
                                        App.Context.SaveChanges();

                                    new Windows.NewMessageBox(Window.GetWindow(this), "Успешно!", $"Пользователь {LoginBox.Text} успешно создан!").ShowDialog();

                                    NavigationService.Navigate(new LoginPage());

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

        void HideShowPlaceholder(TextBox sender, TextBlock textBlock)
        {

            var box = sender;

            if (box.Text.Length > 0)
            {
                textBlock.Visibility = Visibility.Hidden;
            }
            else
            {
                textBlock.Visibility = Visibility.Visible;
            }
        }

        void HideShowPlaceholder(PasswordBox sender, TextBlock textBlock)
        {

            var box = sender;

            if (box.Password.Length > 0)
            {
                textBlock.Visibility = Visibility.Hidden;
            }
            else
            {
                textBlock.Visibility = Visibility.Visible;
            }
        }


        int LoginTextCount = 0;


        private void LoginTextChanged(object sender, TextChangedEventArgs e)
        {
            HideShowPlaceholder((TextBox)sender, PlaceholderLogin);
            if(LoginBox.Text.Length > 0)
            {
                if (LoginTextCount == 0)
                {
                    ChangeProgressBar(ProgressBarAccount.Value + 7.5);
                }

                LoginTextCount++;
            }
            else
            {
                LoginTextCount = 0;
                ChangeProgressBar(ProgressBarAccount.Value - 7.5);
            }
        }

        int PasswordTextCount = 0;
        private void PasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
        {
            HideShowPlaceholder((PasswordBox)sender, PlaceholderPassword);

            if(PasswordBox.Password.Length > 0)
            {
                if (PasswordTextCount == 0)
                {
                    ChangeProgressBar(ProgressBarAccount.Value + 15);
                }

                PasswordTextCount++;
            }
            else
            {
                PasswordTextCount = 0;
                ChangeProgressBar(ProgressBarAccount.Value + 15);
            }
            
        }


        int PhoneTextCount = 0;
        private void PhoneBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            HideShowPlaceholder((TextBox)sender, PlaceholderPhone);

            if (PhoneBox.Text.Length > 0)
            {
                PhoneTextBlock.Text = PhoneBox.Text;
                PhonePanel.Visibility = Visibility.Visible;
                AnimateBorderOpacity(PhonePanel);

                if (PhoneTextCount == 0)
                {
                    ChangeProgressBar(ProgressBarAccount.Value + 15);
                }

                PhoneTextCount++;

            }
            else
            {
                PhoneTextCount = 0;
                ChangeProgressBar(ProgressBarAccount.Value - 15);
                PhoneTextBlock.Text = "";
                PhonePanel.Visibility = Visibility.Hidden;
            }

        }

        BitmapImage ConvertBase64ToImage(string value)
        {
            // строка с данными в формате Base64
            string base64String = value;

            // преобразование строки в массив байтов
            byte[] imageBytes = Convert.FromBase64String(base64String);

            // создание потока из массива байтов
            using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                // создание объекта BitmapImage
                BitmapImage bitmapImage = new BitmapImage();

                // установка свойства StreamSource объекта BitmapImage в поток из массива байтов
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                // установка свойства Source элемента Image WPF в объект BitmapImage
                return bitmapImage;
            }

        }

        private void AnimateBorderOpacity(DependencyObject _object)
        {
            var storyboard = new Storyboard();
            var doubleAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1.5),
                FillBehavior = FillBehavior.Stop // изменение значения FillBehavior
            };
            Storyboard.SetTarget(doubleAnimation, _object);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(Border.OpacityProperty));
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();
        }

        private void AnimateBorderOpacityTwo(DependencyObject _object)
        {
            var storyboard = new Storyboard();
            var doubleAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.2),
                FillBehavior = FillBehavior.Stop // изменение значения FillBehavior
            };
            Storyboard.SetTarget(doubleAnimation, _object);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(Border.OpacityProperty));
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();
        }

        int AvatarCount = 0;

        private void AvatarsComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var box = (ComboBox)sender;
            if (box.SelectedItem != null)
            {
                var selectedAvatar = (KeyValuePair<int,ImageSource>)box.SelectedItem;
                PlaceholderAvatar.Text = $"Выбрана аватарка \"{selectedAvatar.Key}\"";

                RoundedAvatarImage.Source = selectedAvatar.Value;

                RoundedAvatar.Visibility = Visibility.Visible;

                AnimateBorderOpacity(RoundedAvatar);

                PlaceholderAvatar.Visibility = Visibility.Visible;

                if (AvatarCount == 0)
                {
                    ChangeProgressBar(ProgressBarAccount.Value + 20);
                }

                AvatarCount++;

            }
            else
            {

                AvatarCount = 0;

                PlaceholderAvatar.Visibility = Visibility.Hidden;

                ChangeProgressBar(ProgressBarAccount.Value - 20);

            }
        }

        int CountryesCount = 0;

        private void CountryesComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var box = (ComboBox)sender;
            if (box.SelectedItem != null)
            {
                var selectedAvatar = (Messenger_Country)box.SelectedItem;

                CountryTextBlock.Text = selectedAvatar.Country;

                CountryPanel.Visibility = Visibility.Visible;

                AnimateBorderOpacity(CountryPanel);

                PlaceholderAvatar.Visibility = Visibility.Visible;

                if((CityComboBox.SelectedItem as Messenger_City) != null)
                {
                    if ((CityComboBox.SelectedItem as Messenger_City).IdCountry != selectedAvatar.Id)
                    {
                        CityComboBox.SelectedIndex = 0;

                        CityComboBox.SelectedValue = "Город";

                        CityComboBox.Text = "Город";
                    }
                }

                

                CityComboBox.ItemsSource = cities.Where(x=>x.IdCountry==selectedAvatar.Id).ToList();

                if (CountryesCount == 0)
                {
                    ChangeProgressBar(ProgressBarAccount.Value + 20);
                }
                CountryesCount++;


            }
            else
            {
                CountryPanel.Visibility = Visibility.Hidden;
                CountryesCount = 0;
                ChangeProgressBar(ProgressBarAccount.Value - 20);

            }

        }

        int DescriptionTextCount = 0;
        private void DescriptionBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            HideShowPlaceholder((TextBox)sender, PlaceholderDesciption);

            if(DescriptionBox.Text.Length > 0)
            {
                DescriptionPanel.Visibility = Visibility.Visible;
                DescriptionTextBlock.Text = DescriptionBox.Text;
                AnimateBorderOpacityTwo(DescriptionPanel);

                if (DescriptionTextCount == 0)
                {
                    ChangeProgressBar(ProgressBarAccount.Value + 15);
                }

                DescriptionTextCount++;
            }
            else
            {
                DescriptionTextCount = 0;
                ChangeProgressBar(ProgressBarAccount.Value - 15);
                DescriptionPanel.Visibility = Visibility.Hidden;
            }
            
        }

        private void CityComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCity = (ComboBox)sender;

            var City = selectedCity.SelectedItem as Messenger_City;

            if(selectedCity.SelectedItem != null)
            {

                if(CountryesComboBox.SelectedItem != null)
                {
                    CountryTextBlock.Text = City.Messenger_Country.Country +", " + City.City;
                    AnimateBorderOpacity(CountryTextBlock);
                }
                else
                {
                    CountryesComboBox.SelectedItem = countryes.FirstOrDefault(x=>x.Id==City.IdCountry);
                    CountryTextBlock.Text = City.Messenger_Country.Country + ", " + City.City;
                }

            }

        }

        int NicnameTextCount = 0;

        private void NicknameBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            HideShowPlaceholder((TextBox)sender, PlaceholderNickname);
            if (NicknameBox.Text.Length > 0)
            {
                LoginTextBlock.Text = NicknameBox.Text;
                LoginTextBlock.Visibility = Visibility.Visible;
                AnimateBorderOpacityTwo(LoginTextBlock);

                if (NicnameTextCount == 0)
                {
                    ChangeProgressBar(ProgressBarAccount.Value + 7.5);
                }

                NicnameTextCount++;



            }
            else
            {
                LoginTextBlock.Text = "";
                NicnameTextCount = 0;
                ChangeProgressBar(ProgressBarAccount.Value - 7.5);
            }
        }

        private void GoBackTextMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GoBack();
        }

    }
}
