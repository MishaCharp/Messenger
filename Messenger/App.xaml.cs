using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Messenger
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static Dictionary<int,ImageSource> avatars { get; set; } = new Dictionary<int, ImageSource>();

        protected override void OnStartup(StartupEventArgs e)
        {
            
            foreach(var avatar in Context.Messenger_Avatar.ToList())
            {
                avatars.Add(
                    avatar.Id, new Converters.methods().ConvertBase64ToImageSource(avatar.Avatar)
                );
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
