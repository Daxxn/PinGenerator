using PinGenerator.Models;
using PinGenerator.ViewModels;
using PinGenerator.Views;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PinGenerator
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      public MainViewModel VM { get; private set; }
      public MainWindow()
      {
         VM = new MainViewModel();
         DataContext = VM;
         VM.ExpandEvent += VM_ExpandEvent;
         InitializeComponent();
      }

      private void VM_ExpandEvent(object? sender, bool e)
      {
         SerialExpander.IsExpanded = e;
      }

      private void MicroEditor_Click(object sender, RoutedEventArgs e)
      {
         var view = new MicroEditor();
         view.ShowDialog();
      }

      private void NewProject_Click(object sender, RoutedEventArgs e)
      {
         if (VM.IsSaved)
         {
            var newProjView = new NewProjectWindow(VM.NewProject);
            newProjView.ShowDialog();
         }
         else
         {
            if (MessageBox.Show("Project is NOT saved.\nContinue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
               VM.IsSaved = true;
               NewProject_Click(sender, e);
            }
         }
      }

      private void DataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
      {

      }

      private void Component_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (e.AddedItems.Count > 0)
         {
            if (e.AddedItems[0] is Component comp)
            {
               if (comp != VM.SelectedComponent)
               {
                  VM.SelectedComponent = comp;
               }
            }
         }
      }

      private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
      {
         if (e.Changes.Count > 0)
         {
            if (sender is TextBox tb)
            {
               if (string.IsNullOrEmpty(tb.Text)) return;

               var success = uint.TryParse(tb.Text, out uint count);

               if (success && tb.DataContext is Component comp)
               {
                  VM.UpdateComponent(comp, count);
               }
            }
         }
      }
   }
}
