using MVVMLibrary;
using PinGenerator.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinGenerator.Models
{
   public class Pin : Model, IExport
   {
      #region Local Props
      private string _name = "";
      private uint _num = 0;
      private PinType _pinType = 0;
      #endregion

      #region Constructors
      public Pin() { }
      public Pin(string name, uint pinNumber)
      {
         Name = name;
         PinNumber = pinNumber;
      }
      #endregion

      #region Methods
      public override string ToString()
      {
         return $"{Name} : {PinNumber} : {PinType}";
      }

      public string Export()
      {
         if ((int)PinType > 4)
         {
            return $"\t// Comms - {Name} - {PinNumber} - {PinType}";
         }
         return $"\tconst int {Name} = {PinNumber};";
      }
      #endregion

      #region Full Props
      public string Name
      {
         get => _name;
         set
         {
            var output = value.ToUpper();
            if (output.Contains('-'))
            {
               output = output.Replace('-', '_');
            }
            _name = output;
            OnPropertyChanged();
         }
      }

      public uint PinNumber
      {
         get => _num;
         set
         {
            _num = value;
            OnPropertyChanged();
         }
      }

      public PinType PinType
      {
         get => _pinType;
         set
         {
            _pinType = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
