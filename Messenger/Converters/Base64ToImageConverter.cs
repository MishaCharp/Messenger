using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Messenger.Converters
{

    public class methods
    {

        public ImageSource ConvertBase64ToImageSource(string base64)
        {
            byte[] imageBytes = Convert.FromBase64String(base64);

            BitmapImage image = new BitmapImage();
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
            }

            return image;
        }

    }

    public class Base64ToImageConverter : IValueConverter
    {



        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string base64 = (string)value;
            if (string.IsNullOrEmpty(base64))
                return null;

            // переименование функции внутри метода Convert
            var a = new methods();
            ImageSource imageSource = a.ConvertBase64ToImageSource(base64);

            return imageSource;
        }



        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
