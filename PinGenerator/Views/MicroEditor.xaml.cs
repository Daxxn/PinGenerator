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
         if (sender is Button btn)
         {
            if (btn.DataContext is MicroController micro)
            {
               VM.SelectedSerial = micro.AddSerial("NEW");
            }
         }
      }

      private void RemSerial_Click(object sender, RoutedEventArgs e)
      {
         if (VM.SelectedSerial is null) return;

         if (sender is Button btn)
         {
            if (btn.DataContext is MicroController micro)
            {
               micro.RemoveSerial(VM.SelectedSerial);
            }
         }
      }

      private void AddSerialPin_Click(object sender, RoutedEventArgs e)
      {
         if (sender is Button btn)
         {
            if (btn.DataContext is Serial serial)
            {
               VM.SelectedSerialPin = serial.AddPin($"P{serial.Pins.Count}", (uint)serial.Pins.Count);
            }
         }
      }

      private void RemSerialPin_Click(object sender, RoutedEventArgs e)
      {
         if (VM.SelectedSerialPin is null) return;

         if (sender is Button btn)
         {
            if (btn.DataContext is Serial serial)
            {
               serial.RemovePin(VM.SelectedSerialPin);
            }
         }
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
