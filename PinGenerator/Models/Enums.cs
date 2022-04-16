using System.ComponentModel;

namespace PinGenerator.Models.Enums
{
   public enum PinType
   {
      [Description("Input")]
      IN,
      [Description("Output")]
      OUT,
      PWM,
      [Description("IO")]
      IO,
   };
}
