using System.ComponentModel;

namespace PinGenerator.Models.Enums
{
   public enum PinType
   {
      [Description("D IN")]
      DIN,
      [Description("D OUT")]
      DOUT,
      [Description("D IO")]
      DIO,
      [Description("A IN")]
      AIN,
      [Description("A OUT")]
      AOUT,
      UART,
      SPI,
      I2C,
      [Description("OneWire")]
      OW,
      [Description("General Comms")]
      GEN_COMMS
   };
}
