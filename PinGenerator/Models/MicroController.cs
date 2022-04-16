using JsonReaderLibrary;
using MVVMLibrary;
using PinGenerator.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PinGenerator.Models
{
   public class MicroController : Model
   {
      #region Local Props
      public static Dictionary<string, MicroController> Micros { get; set; } = new();

      private string _name = "";
      private uint _digitalPinCount = 0;
      private uint _analogPinCount = 0;
      private ObservableCollection<Serial> _serialComponents = new();
      private ObservableCollection<Component> _digitalComponents = new();
      private ObservableCollection<Component> _analogComponents = new();
      #endregion

      #region Constructors
      public MicroController() { }
      #endregion

      #region Methods
      public static void OnStart()
      {
         try
         {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PinGenerator");
            if (!Directory.Exists(path))
            {
               Directory.CreateDirectory(path);
            }
            var filePath = Path.Combine(path, "Micros.json");
            if (!File.Exists(filePath))
            {
               var file = File.Create(filePath);
               file.Close();
            }
            Micros = JsonReader.OpenJsonFile<Dictionary<string, MicroController>>(filePath);
            if (Micros is null)
            {
               Micros = new();
            }
         }
         catch (Exception)
         {
            MessageBox.Show("Unable to load micros");
         }
      }

      public static void OnExit()
      {
         try
         {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PinGenerator", "Micros.json");
            JsonReader.SaveJsonFile(path, Micros, true);
         }
         catch (Exception)
         {
            MessageBox.Show("Unable to save micros");
         }
      }

      public static void SaveMicros()
      {
         OnExit();
      }

      public static void SaveMicros(IEnumerable<MicroController> micros)
      {
         Micros.Clear();
         foreach (var micro in micros)
         {
            Micros.Add(micro.Name, micro);
         }
         OnExit();
      }

      public static void NewMicro(string name, MicroController micro)
      {
         if (Micros.ContainsKey(name))
         {
            Micros[name] = micro;
            return;
         }
         Micros.Add(name, micro);
      }

      public static bool DeleteMicro(string name)
      {
         return Micros.Remove(name);
      }

      public static IEnumerable<string> GetMicroNames()
      {
         return Micros.Keys;
      }

      public static MicroController? GetMicro(string name)
      {
         if (Micros.ContainsKey(name))
         {
            return Micros[name];
         }
         return null;
      }

      public void AddSerial(string name)
      {
         foreach (var ser in Serial)
         {
            if (ser.Name == name) return;
         }

         Serial.Add(new() { Name = name });
      }

      public bool RemoveSerial(Serial ser)
      {
         return Serial.Remove(ser);
      }

      public Component? NewComponent(string name, uint pinCount, int currentType)
      {
         var comp = Component.Create(name, pinCount);
         if (currentType == 0)
         {
            DigitalComponents.Add(comp);
            return comp;
         }
         else if (currentType == 1)
         {
            AnalogComponents.Add(comp);
            return comp;
         }
         else return null;
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

      public ObservableCollection<Component> DigitalComponents
      {
         get => _digitalComponents;
         set
         {
            _digitalComponents = value;
            OnPropertyChanged();
         }
      }

      public ObservableCollection<Component> AnalogComponents
      {
         get => _analogComponents;
         set
         {
            _analogComponents = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
