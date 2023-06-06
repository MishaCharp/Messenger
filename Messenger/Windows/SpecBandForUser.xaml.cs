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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Messenger.Windows
{
    /// <summary>
    /// Логика взаимодействия для SpecBandForUser.xaml
    /// </summary>
    public partial class SpecBandForUser : Window
    {

        List<Messenger_Specialization> Specializations = App.Context.Messenger_Specialization.ToList();

        List<Messenger_Band> Bands = App.Context.Messenger_Band.ToList();

        public SpecBandForUser()
        {
            InitializeComponent();

            SpecializationsCBox.ItemsSource = Specializations;
            GroupsCBox.ItemsSource = Bands;
        }

        private void AddBtnClick(object sender, RoutedEventArgs e)
        {

            if (GroupsCBox.SelectedIndex >= 0 && SurnameTBox.Text!="" && NameTBox.Text!="" && PatronymicTBox.Text!="")
            {
                App.CurrentUser.Surname = SurnameTBox.Text;
                App.CurrentUser.Name = NameTBox.Text;
                App.CurrentUser.Patronymic = PatronymicTBox.Text;
                App.CurrentUser.IdBand = (GroupsCBox.SelectedItem as Messenger_Band).Id;
                App.Context.SaveChanges();
                new NewMessageBox(GetWindow(this), "Успешно!").ShowDialog();
                this.Close();

            }
            else
            {
                new NewMessageBox(GetWindow(this), "Заполните все данные!").ShowDialog();
            }

        }

        private void SpecializationChanged(object sender, SelectionChangedEventArgs e)
        {
            var _spec = (sender as ComboBox).SelectedItem as Messenger_Specialization;

            GroupsCBox.ItemsSource = Bands.Where(x => x.IdSpecialization == _spec.Id);

        }

        private void BandChanged(object sender, SelectionChangedEventArgs e)
        {
            var _band = (sender as ComboBox).SelectedItem as Messenger_Band;

            if(_band != null )
            {
                SpecializationsCBox.SelectedItem = Specializations.FirstOrDefault(x => x.Id == _band.IdSpecialization);
            }

        }
    }
}
