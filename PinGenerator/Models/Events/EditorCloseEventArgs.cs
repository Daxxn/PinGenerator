using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinGenerator.Models.Events
{
   public class EditorCloseEventArgs : EventArgs
   {
      public IEnumerable<MicroController> Micros { get; init; }

      public EditorCloseEventArgs(IEnumerable<MicroController> micros)
      {
         Micros = micros;
      }
   }
}
