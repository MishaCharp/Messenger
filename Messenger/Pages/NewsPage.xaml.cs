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
    /// Логика взаимодействия для NewsPage.xaml
    /// </summary>
    public partial class NewsPage : Page
    {

        List<Entites.Messenger_Post> postList = new List<Entites.Messenger_Post>();

        public NewsPage()
        {
            InitializeComponent();

            RefreshData();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {

            var Post = (sender as Button).DataContext as Entites.Messenger_Post;

            if(App.Context.Messenger_SavedPost.FirstOrDefault(x=>x.IdUser == Post.IdUser && x.IdPost==Post.Id) != null)
            {
                new Windows.NewMessageBox(Window.GetWindow(this), "Ошибка", "Пост уже добавлен в сохраненные!").ShowDialog();
            }
            else
            {
                App.Context.Messenger_SavedPost.Add(new Entites.Messenger_SavedPost
                {
                    IdPost = Post.Id,
                    IdUser = App.CurrentUser.Id
                });
                App.Context.SaveChanges();

                RefreshData();

            }

        }

        private void RefreshData()
        {
            postList = App.Context.Messenger_Post.OrderByDescending(x => x.DateTime).ToList();
            PostLView.ItemsSource = null;
            PostLView.ItemsSource = postList;
        }
    }
}
