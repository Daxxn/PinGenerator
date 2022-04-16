using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinGenerator.Models
{
   public class Component : Model, IExport
   {
      #region Local Props
      private string _name = "IC";
      private ObservableCollection<Pin> _pins = new();
      private uint _pinCount = 0;
      private Guid _id = Guid.NewGuid();
      #endregion

      #region Constructors
      public Component() { }
      #endregion

      #region Methods
      public static Component Create(string name, uint pinCount)
      {
         Component component = new()
         {
            Name = name,
            PinCount = pinCount
         };
         return component;
      }

      public void AutoGenPins()
      {
         if(Pins.Count == 0)
         {
            for (uint i = 0; i < PinCount; i++)
            {
               Pin pin = new($"P{i}", i);
               Pins.Add(pin);
            }
         }
         else
         {
            if (PinCount > Pins.Count)
            {
               for (int i = Pins.Count; i < PinCount; i++)
               {
                  Pin pin = new($"P{i}", (uint)i);
                  Pins.Add(pin);
               }
            }
         }
      }

      public string Export()
      {
         return $"namespace {Name}Pins {{";
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

      public ObservableCollection<Pin> Pins
      {
         get => _pins;
         set
         {
            _pins = value;
            OnPropertyChanged();
         }
      }

      public uint PinCount
      {
         get => _pinCount;
         set
         {
            _pinCount = value;
            OnPropertyChanged();
         }
      }

      public Guid ID
      {
         get => _id;
         set
         {
            _id = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
