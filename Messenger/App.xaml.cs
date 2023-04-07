using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Messenger
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static void AnimateBorderOpacityFast(DependencyObject _object)
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

        public static void AnimateBorderOpacity(DependencyObject _object)
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

        public static Dictionary<int,ImageSource> avatars { get; set; } = new Dictionary<int, ImageSource>();

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                foreach (var avatar in Context.Messenger_Avatar.ToList())
                {
                    avatars.Add(
                        avatar.Id, new Converters.methods().ConvertBase64ToImageSource(avatar.Avatar)
                    );
                }
            }
            catch(Exception ex)
            {
                new Windows.NewMessageBox(Window.GetWindow(Application.Current.MainWindow), "Ошибка", ex.Message).ShowDialog();
            }



            base.OnStartup(e);
        }

        public static Entites.user2_dbEntities1 Context { get; set; } = new Entites.user2_dbEntities1()
        {

        };

        public static Entites.Messenger_User CurrentUser { get; set; } = new Entites.Messenger_User() 
        {
            
        };

    }
}
