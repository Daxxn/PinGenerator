using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace PinGenerator.Utils.Converters
{
   public class BoolColorConverter : IValueConverter
   {
      private SolidColorBrush NullColor { get; init; } = new(Color.FromRgb(0, 0, 255));
      private SolidColorBrush TrueColor { get; init; } = new(Color.FromRgb(0, 255, 0));
      private SolidColorBrush FalseColor { get; init; } = new(Color.FromRgb(255, 0, 0));

      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (value is bool b)
         {
            return b ? TrueColor : FalseColor;
         }
         return NullColor;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         return value;
      }
   }
}
