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
    /// Логика взаимодействия для NewMessageBox.xaml
    /// </summary>
    public partial class NewMessageBox : Window
    {
        public NewMessageBox(Window Parent,string title, string description)
        {
            InitializeComponent();
            this.Owner = Parent;
            TitleMessageBox.Text = title;
            DescriptionMessageBox.Text = description;
        }

        public NewMessageBox(Window Parent,string description)
        {
            InitializeComponent();
            this.Owner = Parent;
            TitleMessageBox.Text = "";
            DescriptionMessageBox.Text = description;
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
