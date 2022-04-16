using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinGenerator.Models
{
   public class SerialComponent : Model
   {
      #region Local Props
      private string _name = "";
      private int? _address = null;
      private Pin? _selectPin = null;
      #endregion

      #region Constructors
      public SerialComponent() { }
      #endregion

      #region Methods
      public static SerialComponent Create(string name, int address)
      {
         return new SerialComponent() { Name = name, Address = address };
      }
      public static SerialComponent Create(string name, Pin selectPin)
      {
         return new SerialComponent() { Name = name, SelectPin = selectPin };
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

      public int? Address
      {
         get => _address;
         set
         {
            _address = value;
            _selectPin = null;
            OnPropertyChanged();
         }
      }

      public Pin? SelectPin
      {
         get => _selectPin;
         set
         {
            _selectPin = value;
            _address = null;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
