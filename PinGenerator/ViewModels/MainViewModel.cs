using MVVMLibrary;
using PinGenerator.Models;
using JsonReaderLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.IO;

namespace PinGenerator.ViewModels
{
   public class MainViewModel : ViewModel
   {
      #region Local Props
      private Project _proj = new();
      private Component? _selectedComp = null;
      private Pin? _selectedPin = null;

      private string? _newCompName = null;
      private uint? _newCompPins = null;

      private bool _isSaved = false;

      #region Commands
      public Command NewProjectCmd { get; init; }
      public Command OpenProjectCmd { get; init; }
      public Command SaveProjectCmd { get; init; }
      public Command NewCompCmd { get; init; }
      public Command AutoGenPinsCmd { get; init; }
      public Command BrowseExportCmd { get; init; }
      public Command ExportCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public MainViewModel()
      {
         #region Test Models
         Project = new()
         {
            Name = "Project Test",
            Components = new()
            {
               Component.Create("Test 1", 16),
               Component.Create("Test 2", 8),
               Component.Create("Test 1", 32),
            }
         };
         #endregion

         #region Commands Init
         NewProjectCmd   = new(NewProject);
         OpenProjectCmd  = new(OpenProject);
         SaveProjectCmd  = new(SaveProject);
         AutoGenPinsCmd  = new(AutoGenPins);
         NewCompCmd      = new(NewComponent);
         BrowseExportCmd = new(BrowseExport);
         ExportCmd       = new(ExportCode);
         #endregion
      }
      #endregion

      #region Methods
      private void NewProject()
      {
         if (IsSaved)
         {
            Project = new();
            SelectedComponent = null;
            SelectedPin = null;
            IsSaved = false;
         }
         else
         {
            if (MessageBox.Show("Project is NOT saved.\nContinue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
               Project = new();
               SelectedComponent = null;
               SelectedPin = null;
               IsSaved = false;
            }
         }
      }

      private void SaveProject()
      {
         try
         {
            if (Project.Path is null)
            {
               SaveFileDialog dialog = new()
               {
                  Title = "Save Project",
                  DefaultExt = ".pin",
                  Filter = "*.pin|Pin File|*.*|All Files"
               };

               if (dialog.ShowDialog() == true)
               {
                  Project.Path = dialog.FileName;
                  Project.Name = Path.GetFileNameWithoutExtension(dialog.FileName);
               }
            }
            JsonReader.SaveJsonFile(Project.Path, Project, true);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "Save Error");
         }
      }

      private void OpenProject()
      {
         try
         {
            if (IsSaved)
            {
               OpenFileDialog dialog = new()
               {
                  Title = "Open Project",
                  Filter = "*.pin|Pin File|*.*|All Files",
                  InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
               };

               if (dialog.ShowDialog() == true)
               {
                  Project = JsonReader.OpenJsonFile<Project>(dialog.FileName);
               }
            }
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "Open Error");
         }
      }

      private void NewComponent()
      {
         if (NewCompName != null && NewCompPins != null)
         {
            Project.NewComponent(NewCompName, (uint)NewCompPins);
         }
      }

      private void AutoGenPins()
      {
         if (SelectedComponent is null) return;
         SelectedComponent.AutoGenPins();
      }

      private void BrowseExport()
      {
         SaveFileDialog dialog = new()
         {
            FileName = Project.ExportPath,
            Title = "Export Path",
            AddExtension = true,
            DefaultExt = ".h",
            CustomPlaces = new List<FileDialogCustomPlace>()
            {
               new(@"C:\Users\Daxxn\Code\Arduino"),
               new(@"C:\Users\Daxxn\Code\Arduino\Projects")
            }
         };

         if (dialog.ShowDialog() == true)
         {
            Project.ExportPath = dialog.FileName;
         }
      }

      private void ExportCode()
      {
         if (Project is null) return;
         CodeExporter.Export(Project);
      }
      #endregion

      #region Full Props
      public string Title
      {
         get
         {
            return $"Pin Generator : {Project?.Name}";
         }
      }

      public Project Project
      {
         get => _proj;
         set
         {
            _proj = value;
            OnPropertyChanged();
         }
      }

      public Component? SelectedComponent
      {
         get => _selectedComp;
         set
         {
            _selectedComp = value;
            OnPropertyChanged();
         }
      }

      public Pin? SelectedPin
      {
         get => _selectedPin;
         set
         {
            _selectedPin = value;
            OnPropertyChanged();
         }
      }

      public bool IsSaved
      {
         get => _isSaved;
         set
         {
            _isSaved = value;
            OnPropertyChanged();
         }
      }

      #region New Component Props
      public string? NewCompName
      {
         get => _newCompName;
         set
         {
            _newCompName = value;
            OnPropertyChanged();
         }
      }

      public uint? NewCompPins
      {
         get => _newCompPins;
         set
         {
            _newCompPins = value;
            OnPropertyChanged();
         }
      }
      #endregion
      #endregion
   }
}
