using PinGenerator.Models;
using PinGenerator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PinGenerator.Views
{
   /// <summary>
   /// Interaction logic for MicroEditor.xaml
   /// </summary>
   public partial class MicroEditor : Window
   {
      public MicroEditorViewModel VM { get; private set; }
      public MicroEditor()
      {
         VM = new();
         DataContext = VM;
         InitializeComponent();
      }

      private void AddSerial_Click(object sender, RoutedEventArgs e)
      {
         VM.AddSerialCmd.Execute(null);
      }

      private void RemSerial_Click(object sender, RoutedEventArgs e)
      {
         VM.RemSerialCmd.Execute(null);
      }

      private void AddSerialPin_Click(object sender, RoutedEventArgs e)
      {
         VM.AddSerialPinCmd.Execute(null);
      }

      private void RemSerialPin_Click(object sender, RoutedEventArgs e)
      {
         VM.RemSerialPinCmd.Execute(null);
      }

      private void Serial_Selected(object sender, SelectionChangedEventArgs e)
      {
         if (e.AddedItems.Count > 0)
         {
            if (e.AddedItems[0] is Serial ser)
            {
               VM.SelectedSerial = ser;
            }
            if (e.AddedItems[0] is Pin pin)
            {
               VM.SelectedSerialPin = pin;
            }
         }
      }

      private void Close_Click(object sender, RoutedEventArgs e)
      {
         Close();
      }
   }
}
