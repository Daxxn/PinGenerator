using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinGenerator.Models
{
   public class MicroControllerTemplate : Model
   {
      #region Local Props
      private string _name = "";
      private uint _digitalPinCount = 0;
      private uint _analogPinCount = 0;
      private ObservableCollection<Serial> _serialComponents = new();
      #endregion

      #region Constructors
      public MicroControllerTemplate() { }
      #endregion

      #region Methods

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

      public uint DigitalPinCount
      {
         get => _digitalPinCount;
         set
         {
            _digitalPinCount = value;
            OnPropertyChanged();
         }
      }

      public uint AnalogPinCount
      {
         get => _analogPinCount;
         set
         {
            _analogPinCount = value;
            OnPropertyChanged();
         }
      }

      public ObservableCollection<Serial> Serial
      {
         get => _serialComponents;
         set
         {
            _serialComponents = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
