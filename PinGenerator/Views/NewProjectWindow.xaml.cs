using PinGenerator.Models;
using PinGenerator.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
   /// Interaction logic for NewProjectWindow.xaml
   /// </summary>
   public partial class NewProjectWindow : Window
   {
      public NewProjectViewModel VM { get; set; }
      public NewProjectWindow(NewProjectViewModel.CreateProjectCompleted completed)
      {
         VM = new(completed);
         DataContext = VM;
         InitializeComponent();
         MicroEditorViewModel.SaveMicrosEvent += MicroEditorViewModel_SaveMicrosEvent;
      }

      private void MicroEditorViewModel_SaveMicrosEvent(object? sender, EventArgs e)
      {
         //MicrosCombo.ItemsSource = MicroController.Micros.Keys;
         //MicrosCombo.UpdateLayout();
      }

      private void Create_Click(object sender, RoutedEventArgs e)
      {
         MicroEditorViewModel.SaveMicrosEvent -= MicroEditorViewModel_SaveMicrosEvent;
         Close();
      }

      private void OpenMicroEditor_Click(object sender, RoutedEventArgs e)
      {
         var EditorView = new MicroEditor();
         EditorView.ShowDialog();
         VM.MicroNames = new(MicroController.Micros.Keys);
      }
   }
}
