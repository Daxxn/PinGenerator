using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace PinGenerator.Utils.Converters
{
   public class NullVisibilityConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (parameter is null) return value is null ? Visibility.Hidden : Visibility.Visible;
         return value is not null ? Visibility.Hidden : Visibility.Visible;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         return value;
      }
   }
}
