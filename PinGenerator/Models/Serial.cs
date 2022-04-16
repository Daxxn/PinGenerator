using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinGenerator.Models
{
   public class Serial : Model
   {
      #region Local Props
      private string _name = "";
      private ObservableCollection<Pin> _pins = new();
      private ObservableCollection<SerialComponent> _components = new();
      #endregion

      #region Constructors
      public Serial() { }
      #endregion

      #region Methods
      public void SetAllPins(Pin[] pins)
      {
         Pins = new(pins);
      }

      public void SetPin(Pin pin)
      {
         for (int i = 0; i < Pins.Count; i++)
         {
            if (Pins[i].PinNumber == pin.PinNumber)
            {
               Pins[i] = pin;
               return;
            }
         }
         Pins.Add(pin);
      }

      public Pin AddPin(string name, uint pinNumber)
      {
         var pin = new Pin(name, pinNumber);
         Pins.Add(pin);
         return pin;
      }

      public bool RemovePin(Pin pin)
      {
         return Pins.Remove(pin);
      }

      public SerialComponent? NewComponent(string name, int address)
      {
         foreach (var c in Components)
         {
            if (c.Name == name)
            {
               return null;
            }
         }

         var comp = SerialComponent.Create(name, address);
         Components.Add(comp);
         return comp;
      }

      public bool RemoveComponent(string name)
      {
         var found = Components.FirstOrDefault(comp => comp.Name == name);
         if (found is null) return false;
         return Components.Remove(found);
      }
      #endregion

      #region Full Props
      public ObservableCollection<Pin> Pins
      {
         get => _pins;
         set
         {
            _pins = value;
            OnPropertyChanged();
         }
      }

      public ObservableCollection<SerialComponent> Components
      {
         get => _components;
         set
         {
            _components = value;
            OnPropertyChanged();
         }
      }

      public string Name
      {
         get => _name;
         set
         {
            _name = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
