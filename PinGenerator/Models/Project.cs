using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinGenerator.Models
{
   public class Project : Model
   {
      #region Local Props
      private string _name = "Empty Project";
      private string? _path = null;
      private string? _exportPath = null;
      //private ObservableCollection<Component> _comps = new();
      private MicroController _micro = new();
      #endregion

      #region Constructors
      public Project() { }
      #endregion

      #region Methods
      public static Project Create(string name, string path, MicroController micro)
      {
         return new()
         {
            Name = name,
            Path = path,
            Micro = micro,
         };
      }
      public static Project Create(string name, string path, string exportPath, MicroController micro)
      {
         return new()
         {
            Name = name,
            Path = path,
            Micro = micro,
            ExportPath = exportPath,
         };
      }
      #endregion

      #region Full Props
      public string Name
      {
         get => _name;
         set
         {
            _name = value;
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

      public string? ExportPath
      {
         get => _exportPath;
         set
         {
            _exportPath = value;
            OnPropertyChanged();
         }
      }

      //public ObservableCollection<Component> Components
      //{
      //   get => _comps;
      //   set
      //   {
      //      _comps = value;
      //      OnPropertyChanged();
      //   }
      //}

      public MicroController Micro
      {
         get => _micro;
         set
         {
            _micro = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
