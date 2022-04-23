using MyShop.Ultilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyShop.Converters
{
    internal class GenderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            int number = (int)value;
            if (AppConfigUltility.CurrentLanguage.Equals("VI"))
            {
                return number == 1 ? "Nam" : "Nữ";
            }
            else
            {
                return number == 1 ? "Male" : "Female";
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

       
    }
}