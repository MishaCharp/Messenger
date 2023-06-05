//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Messenger.Entites
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Media;

    public partial class Messenger_Message
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public Nullable<int> IdSticker { get; set; }
        public int IdDialog { get; set; }
        public System.DateTime Time { get; set; }
        public Nullable<int> LastIdSenderUser { get; set; }
        public string LastNicknameSenderUser { get; set; }

        public System.Windows.Media.Brush BgColor
        {
            get
            {
                string colorString = LastNicknameSenderUserTrim == "Я" ? "#687e9c" : "#40536d"; // Здесь можно задать нужные цвета в формате "#RRGGBB"

                System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorString);
                SolidColorBrush brush = new SolidColorBrush(color);

                return brush;
            }
        }



        public string LastNicknameSenderUserTrim
        {
            get
            {
                if (LastNicknameSenderUser != App.CurrentUser.Nickname)
                {
                    return LastNicknameSenderUser;
                }
                else
                {
                    return "Я";
                }
            }
        }

        public System.Windows.Media.Brush AccountColor
        {
            get
            {
                if (App.CurrentUser.Id == LastIdSenderUser)
                {
                    return System.Windows.Media.Brushes.LightBlue;
                }
                else
                {
                    return System.Windows.Media.Brushes.LightBlue;
                }
            }
        }

        public virtual Messenger_Dialog Messenger_Dialog { get; set; }
        public virtual Messenger_Sticker Messenger_Sticker { get; set; }
    }
}
