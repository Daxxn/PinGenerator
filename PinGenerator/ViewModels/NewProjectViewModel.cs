using Microsoft.Win32;
using MVVMLibrary;
using PinGenerator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinGenerator.ViewModels
{
   public class NewProjectViewModel : ViewModel
   {
      #region Local Props
      private string _projName = "New Project";
      private string? _path = null;
      private string? _exportCodePath = null;
      private string? _exportDocPath = null;
      private ObservableCollection<string> _micros = new(MicroController.Micros.Keys);
      private string? _selectedMicro = null;

      public delegate void CreateProjectCompleted(Project proj);

      public CreateProjectCompleted createdDelegate { get; private set; }

      #region Commands
      public Command CreateProjectCmd { get; init; }
      public Command BrowsePathCmd { get; init; }
      public Command BrowseExpCodePathCmd { get; init; }
      public Command BrowseExpDocPathCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public NewProjectViewModel(CreateProjectCompleted del)
      {
         createdDelegate = del;

         CreateProjectCmd = new(CreateProject);
         BrowsePathCmd = new(BrowsePath);
         BrowseExpCodePathCmd = new(BrowseExportCodePath);
         BrowseExpDocPathCmd = new(BrowseExportDocPath);
      }
      #endregion

      #region Methods
      private void CreateProject()
      {
         if (SelectedMicro is null || Path is null || ExportCodePath is null) return;

         var micro = MicroController.GetMicro(SelectedMicro);

         if (micro is null) return;
         var newMicro = MicroController.CopyMicro(micro);

         createdDelegate(Project.Create(ProjectName, Path, ExportCodePath, ExportDocPath, newMicro));
      }

      private void BrowsePath()
      {
         SaveFileDialog dialog = new()
         {
            AddExtension = true,
            DefaultExt = ".pin",
            Filter = Const.FileFilters,
            Title = "Select Project Save File",
            FileName = ProjectName,
            CustomPlaces = Const.CustomProjectDirs
         };

         if (dialog.ShowDialog() == true)
         {
            Path = dialog.FileName;
         }
      }

      private void BrowseExportCodePath()
      {
         SaveFileDialog dialog = new()
         {
            AddExtension = true,
            DefaultExt = ".h",
            Filter = Const.HeaderFilters,
            Title = "Select Project Export File",
            FileName = $"{ProjectName}Pinout.h",
            CustomPlaces = Const.CustomExportDirs
         };

         if (dialog.ShowDialog() == true)
         {
            ExportCodePath = dialog.FileName;
         }
      }

      private void BrowseExportDocPath()
      {
         SaveFileDialog dialog = new()
         {
            AddExtension = true,
            DefaultExt = ".md",
            Filter = Const.MDFilters,
            Title = "Select Project Doc File",
            FileName = $"{ProjectName}Pinout.md",
            CustomPlaces = Const.CustomExportDirs
         };

         if (dialog.ShowDialog() == true)
         {
            ExportDocPath = dialog.FileName;
         }
      }
      #endregion

      #region Full Props
      public ObservableCollection<string> MicroNames
      {
         get => _micros;
         set
         {
            _micros = value;
            OnPropertyChanged();
         }
      }

      public string? SelectedMicro
      {
         get => _selectedMicro;
         set
         {
            _selectedMicro = value;
            OnPropertyChanged();
         }
      }

      public string ProjectName
      {
         get => _projName;
         set
         {
            _projName = value;
            OnPropertyChanged();
         }
      }

      public string? Path
      {
         get => _path;
         set
         {
            _path = value;
            OnPropertyChanged();
         }
      }

      public string? ExportCodePath
      {
         get => _exportCodePath;
         set
         {
            _exportCodePath = value;
            OnPropertyChanged();
         }
      }

      public string? ExportDocPath
      {
         get => _exportDocPath;
         set
         {
            _exportDocPath = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
