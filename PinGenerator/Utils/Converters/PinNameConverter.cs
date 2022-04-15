using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PinGenerator.Utils.Converters
{
   public class PinNameConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (value is string str)
         {
            var output = str.ToUpper();
            if (output.Contains('-'))
            {
               output = str.Replace('-', '_');
            }
            return output;
         }
         return value;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         return value;
      }
   }
}
