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
   public class CurrentPinTypeConverter : IValueConverter
   {
      private int? Parameter { get; set; }
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (value is null) return Visibility.Hidden;
         if (parameter is not null)
         {
            if (value is int val)
            {
               if (Parameter is not null)
               {
                  return Parameter == val ? Visibility.Visible : Visibility.Hidden;
               }
               if (parameter is string param)
               {
                  if (Parameter is null)
                  {
                     var success = int.TryParse(param, out int pInt);
                     if (success)
                     {
                        Parameter = pInt;
                        return pInt == val ? Visibility.Visible : Visibility.Hidden;
                     }
                     return Visibility.Visible;
                  }
               }
            }
         }
         return Visibility.Visible;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         return value;
      }
   }
}
