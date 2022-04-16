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
using PinGenerator.Models.Events;

namespace PinGenerator.ViewModels
{
   public class MainViewModel : ViewModel
   {
      #region Local Props
      public event EventHandler<bool> ExpandEvent = null;
      public static event EventHandler<bool> IsSaveChanged;

      private Project _proj = new();
      private Component? _selectedComp = null;
      private Component? _selectedDigitalComp = null;
      private Component? _selectedAnalogComp = null;
      private Pin? _selectedPin = null;
      private Serial? _selectedSerial = null;

      private string? _newCompName = null;
      private uint? _newCompPins = null;

      private bool _isSaved = true;

      private int _currentPinType = 0;
      private bool _autoGenPins = true;

      #region Commands
      public Command OpenProjectCmd { get; init; }
      public Command SaveProjectCmd { get; init; }
      public Command NewCompCmd { get; init; }
      public Command GenSelectedPinsCmd { get; init; }
      public Command BrowseExportCmd { get; init; }
      public Command ExportCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public MainViewModel()
      {
         #region Test Models
         //Project = new()
         //{
         //   Name = "Project Test",
         //   Micro = new()
         //   {
         //      DigitalPinCount = 16,
         //      AnalogPinCount = 8,
         //      DigitalComponents = new()
         //      {
         //         new() { Name = "Test 1", PinCount = 8},
         //         new() { Name = "Test 2", PinCount = 4}
         //      },
         //      AnalogComponents = new()
         //      {
         //         new() { Name = "Test 3", PinCount = 7 },
         //         new() { Name = "Test 4", PinCount = 16 },
         //      },
         //      Serial = new()
         //      {
         //         new() {
         //            Name = "I2C",
         //            Pins = new() { new() { Name = "I2C_SCLK", PinNumber = 8, }, new() { Name = "I2C_SDA", PinNumber = 9 } },
         //            Components  = new()
         //            {
         //               new() { Name = "IO Expander", Address = 42 },
         //               new() { Name = "RTC", Address = 24 }
         //            }
         //         },
         //         new()
         //         {
         //            Name = "SPI",
         //            Pins = new() { new() { Name = "SPI_SCLK", PinNumber=10}, new() { Name ="SPI_MOSI", PinNumber=11}, new() { Name ="SPI_MISO", PinNumber=12} },
         //            Components = new()
         //            {
         //               new() { Name="Temp Sensor", SelectPin = new() { Name ="THERMO_CS", PinNumber=13} }
         //            }
         //         }
         //      }
         //   }
         //};
         #endregion

         #region Commands Init
         OpenProjectCmd  = new(OpenProject);
         SaveProjectCmd  = new(SaveProject);
         GenSelectedPinsCmd = new(GeneratePins);
         NewCompCmd      = new(NewComponent);
         BrowseExportCmd = new(BrowseExport);
         ExportCmd       = new(ExportCode);
         #endregion

         #region Event Init
         IsSaveChanged += MainViewModel_IsSaveChanged;
         #endregion
      }

      private void MainViewModel_IsSaveChanged(object? sender, bool saved)
      {
         IsSaved = saved;
      }
      #endregion

      #region Methods
      public void NewProject(Project proj)
      {
         if (proj is null)
         {
            MessageBox.Show("Project not created...", "Error");
            return;
         }
         Project = proj;
         IsSaved = false;
      }

      private void SaveProject()
      {
         try
         {
            if (Project.Name == "Empty Project")
            {
               if (Project.Micro.AnalogComponents.Count == 0 && Project.Micro.DigitalComponents.Count == 0)
               {
                  MessageBox.Show("Project is empty. Create a new project to save it.");
                  return;
               }
            }
            if (Project.Path is null)
            {
               SaveFileDialog dialog = new()
               {
                  Title = "Save Project",
                  DefaultExt = ".pin",
                  Filter = Const.FileFilters,
                  CustomPlaces = Const.CustomProjectDirs,
               };

               if (dialog.ShowDialog() == true)
               {
                  Project.Path = dialog.FileName;
                  Project.Name = Path.GetFileNameWithoutExtension(dialog.FileName);
               }
               else
               {
                  return;
               }
            }
            JsonReader.SaveJsonFile(Project.Path, Project, true);
            IsSaved = true;
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
                  Filter = Const.FileFilters,
                  InitialDirectory = Const.CustomProjectDirs[0].Path,
                  CustomPlaces = Const.CustomProjectDirs,
               };

               if (dialog.ShowDialog() == true)
               {
                  Project = JsonReader.OpenJsonFile<Project>(dialog.FileName);
               }
            }
            else
            {
               if (MessageBox.Show("Project is NOT saved.\nContinue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
               {
                  IsSaved = true;
                  OpenProject();
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
            if (CurrentPinType == 2)
            {
               if (SelectedSerial is null) return;
               SelectedSerial.NewComponent(NewCompName, (int)NewCompPins);
               IsSaved = false;
               return;
            }
            var newComp = Project.Micro.NewComponent(NewCompName, (uint)NewCompPins, CurrentPinType);
            SelectedComponent = newComp;
            if (CurrentPinType == 0)
            {
               SelectedDigitalComponent = newComp;
            }
            else if (CurrentPinType == 1)
            {
               SelectedAnalogComponent = newComp;
            }
            if (AutoGenPins)
            {
               GeneratePins();
            }
            IsSaved = false;
         }
      }

      public void UpdateComponent(Component comp, uint count)
      {
         if (comp is null) return;

         comp.PinCount = count;
         comp.AutoGenPins();

         if (CurrentPinType == 0)
         {
            if (Project.Micro.DigitalComponents.Contains(comp))
            {
               var c = Project.Micro.DigitalComponents.First(c => c.ID == comp.ID);
               c = comp;
            }
         }
         else if (CurrentPinType == 1)
         {
            if (Project.Micro.AnalogComponents.Contains(comp))
            {
               var c = Project.Micro.AnalogComponents.First(c => c.ID == comp.ID);
               c = comp;
            }
         }

         IsSaved = false;
      }

      private void GeneratePins()
      {
         if (SelectedComponent is null) return;
         SelectedComponent.AutoGenPins();
         IsSaved = false;
      }

      private void BrowseExport()
      {
         SaveFileDialog dialog = new()
         {
            FileName = string.IsNullOrEmpty(Project.ExportPath) ? $"{Project.Name}Pinout.h" : Project.ExportPath,
            Title = "Export Path",
            AddExtension = true,
            DefaultExt = ".h",
            Filter = Const.HeaderFilters,
            CustomPlaces = Const.CustomExportDirs,
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
            return $"Pin Generator : {Project?.Name}{(!IsSaved ? "*" : "")}";
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

      public Component? SelectedDigitalComponent
      {
         get => _selectedDigitalComp;
         set
         {
            _selectedDigitalComp = value;
            if (CurrentPinType == 0)
            {
               SelectedComponent = value;
            }
            OnPropertyChanged();
         }
      }

      public Component? SelectedAnalogComponent
      {
         get => _selectedAnalogComp;
         set
         {
            _selectedAnalogComp = value;
            if (CurrentPinType == 1)
            {
               SelectedComponent = value;
            }
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

      public Serial? SelectedSerial
      {
         get => _selectedSerial;
         set
         {
            _selectedSerial = value;
            ExpandEvent?.Invoke(this, value is not null);
            OnPropertyChanged();
         }
      }

      public int CurrentPinType
      {
         get => _currentPinType;
         set
         {
            _currentPinType = value;
            if (value == 0)
            {
               SelectedComponent = SelectedDigitalComponent;
            }
            else if (value == 1)
            {
               SelectedComponent = SelectedAnalogComponent;
            }
            OnPropertyChanged();
            OnPropertyChanged(nameof(NewPinTitleText));
            OnPropertyChanged(nameof(SelectedComponent));
         }
      }

      public string NewPinTitleText
      {
         get
         {
            if (CurrentPinType == 2) return "Address Pin";
            else return "Pin Count";
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

      public bool AutoGenPins
      {
         get => _autoGenPins;
         set
         {
            _autoGenPins = value;
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
