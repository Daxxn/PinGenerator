using JsonReaderLibrary;
using MVVMLibrary;
using PinGenerator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PinGenerator.ViewModels
{
   public class MicroEditorViewModel : ViewModel
   {
      #region Local Props
      public static event EventHandler SaveMicrosEvent;

      private ObservableCollection<MicroController> _micros = new();
      private MicroController? _selectedMicro = null;
      private Serial? _selectedSerial = null;
      private Pin? _selectedSerialPin = null;

      #region Commands
      public Command NewMicroCmd { get; init; }
      public Command RemoveMicroCmd { get; init; }
      public Command AddSerialCmd { get; init; }
      public Command RemSerialCmd { get; init; }
      public Command AddSerialPinCmd { get; init; }
      public Command RemSerialPinCmd { get; init; }

      public Command SaveMicrosCmd { get; init; }
      #endregion
      #endregion

      #region Constructors
      public MicroEditorViewModel()
      {
         if (MicroController.Micros.Count > 0)
         {
            Micros = new(MicroController.Micros.Values);
         }

         NewMicroCmd = new(NewMicro);
         RemoveMicroCmd = new(RemoveMicro);
         AddSerialCmd = new(AddSerial);
         RemSerialCmd = new(RemSerial);
         AddSerialPinCmd = new(AddSerialPin);
         RemSerialPinCmd = new(RemSerialPin);
         SaveMicrosCmd = new(SaveMicros);
      }
      #endregion

      #region Methods
      public void EditorClose(object sender, EventArgs e)
      {
         foreach (var micro in Micros)
         {
            MicroController.Micros[micro.Name] = micro;
         }
      }

      private void NewMicro()
      {
         MicroController micro = new();
         Micros.Add(micro);
         SelectedMicro = micro;
      }

      private void RemoveMicro()
      {
         if (SelectedMicro is null) return;
         Micros.Remove(SelectedMicro);
         SelectedMicro = null;
      }

      private void AddSerial()
      {
         if (SelectedMicro is null) return;
         SelectedMicro.AddSerial("Serial");
      }

      private void RemSerial()
      {
         if (SelectedMicro is null || SelectedSerial is null) return;
         SelectedMicro.RemoveSerial(SelectedSerial);
         SelectedSerial = null;
      }

      private void AddSerialPin()
      {
         if (SelectedSerial is null) return;
         SelectedSerial.AddPin($"P{SelectedSerial.Pins.Count}", (uint)SelectedSerial.Pins.Count);
      }

      private void RemSerialPin()
      {
         if (SelectedSerial is null || SelectedSerialPin is null) return;
         SelectedSerial.RemovePin(SelectedSerialPin);
         SelectedSerialPin = null;
      }

      private void SaveMicros()
      {
         if (Micros is null) return;
         MicroController.SaveMicros(Micros);
         SaveMicrosEvent?.Invoke(this, new());
      }
      #endregion

      #region Full Props
      public ObservableCollection<MicroController> Micros
      {
         get => _micros;
         set
         {
            _micros = value;
            OnPropertyChanged();
         }
      }

      public MicroController? SelectedMicro
      {
         get => _selectedMicro;
         set
         {
            _selectedMicro = value;
            OnPropertyChanged();
         }
      }

      public Pin? SelectedSerialPin
      {
         get => _selectedSerialPin;
         set
         {
            _selectedSerialPin = value;
            OnPropertyChanged();
         }
      }

      public Serial? SelectedSerial
      {
         get => _selectedSerial;
         set
         {
            _selectedSerial = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
