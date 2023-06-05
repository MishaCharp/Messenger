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
    /// Логика взаимодействия для SavedPage.xaml
    /// </summary>
    public partial class SavedPage : Page
    {

        List<Entites.Messenger_SavedPost> postList = new List<Entites.Messenger_SavedPost>();


        public SavedPage()
        {
            InitializeComponent();

            RefreshData();
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {

            var Post = (sender as Button).DataContext as Entites.Messenger_SavedPost;

            App.Context.Messenger_SavedPost.Remove(Post);

            App.Context.SaveChanges();

            RefreshData();

        }


        private void RefreshData()
        {
            postList = App.Context.Messenger_SavedPost.Where(x=>x.IdUser==App.CurrentUser.Id).OrderByDescending(x => x.Messenger_Post.DateTime).ToList();
            PostLView.ItemsSource = null;
            PostLView.ItemsSource = postList;
        }


    }
}
