using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Messenger
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static Entites.user2_dbEntities1 Context { get; set; } = new Entites.user2_dbEntities1()
        {

        };

        public static Entites.Messenger_User CurrentUser { get; set; } = new Entites.Messenger_User() 
        {
            
        };

    }
}
