using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinGenerator.Models
{
   public static class MarkdownExporter
   {
      #region Methods
      public static void Export(Project project)
      {
         if (!string.IsNullOrEmpty(project.ExportDocPath))
         {
            StringBuilder sb = BuildHeader(project);

            BuildDigitalComponents(sb, project.Micro.DigitalComponents);
            BuildAnalogComponents(sb, project.Micro.AnalogComponents);
            BuildSerialComponents(sb, project.Micro.Serial);

            BuildFooter(sb);

            var temp = sb.ToString();
            SaveFile(project.ExportDocPath, temp);
         }
      }

      public static async Task ExportAsync(Project project)
      {
         await Task.Run(() =>
         {
            Export(project);
         });
      }

      private static StringBuilder BuildHeader(Project project)
      {
         StringBuilder sb = new();
         sb.AppendLine($"# {project.Name}");
         sb.AppendLine();
         sb.AppendLine($"> > Micro: **{project.Micro.Name}**");
         sb.AppendLine(">");
         sb.AppendLine($"> > Digital Pins: **{project.Micro.DigitalPinCount}**");
         sb.AppendLine("> >");
         sb.AppendLine($"> > Analog Pins: **{project.Micro.AnalogPinCount}**");
         sb.AppendLine(">");
         BuildSerialHeader(sb, project.Micro.Serial);
         sb.AppendLine();
         sb.AppendLine("---");
         sb.AppendLine();

         return sb;
      }

      private static void BuildSerialHeader(StringBuilder sb, IEnumerable<Serial> serial)
      {
         sb.AppendLine($"> > Comms:");
         foreach (var ser in serial)
         {
            sb.AppendLine($"> > - {ser.Name}");
         }
      }

      private static void BuildDigitalComponents(StringBuilder sb, IEnumerable<Component> components)
      {
         sb.AppendLine("## Digital");
         sb.AppendLine();
         foreach (var comp in components)
         {
            sb.AppendLine($"### {comp.Name}");
            sb.AppendLine($"Pin Count: {comp.PinCount}");
            sb.AppendLine();
            sb.AppendLine("| Pin | Name | Type");
            sb.AppendLine("| --- | --- | --- |");
            foreach (var pin in comp.Pins)
            {
               sb.AppendLine($"| {pin.PinNumber} | {pin.Name} | {pin.PinType} |");
            }
            sb.AppendLine();
         }
         sb.AppendLine();
      }

      private static void BuildAnalogComponents(StringBuilder sb, IEnumerable<Component> components)
      {
         sb.AppendLine("## Analog");
         sb.AppendLine();
         foreach (var comp in components)
         {
            sb.AppendLine($"### {comp.Name}");
            sb.AppendLine($"Pin Count: {comp.PinCount}");
            sb.AppendLine();
            sb.AppendLine("| Pin | Name | Type");
            sb.AppendLine("| --- | --- | --- |");
            foreach (var pin in comp.Pins)
            {
               sb.AppendLine($"| {pin.PinNumber} | A{pin.Name} | {pin.PinType} |");
            }
            sb.AppendLine();
         }
         sb.AppendLine();
      }

      private static void BuildSerialComponents(StringBuilder sb, IEnumerable<Serial> serial)
      {
         sb.AppendLine("## Serial");
         sb.AppendLine();
         foreach (var ser in serial)
         {
            sb.AppendLine($"### {ser.Name}");
            sb.AppendLine();
            sb.AppendLine("### Pins");
            sb.AppendLine();
            sb.AppendLine("| Name | Pin |");
            sb.AppendLine("| --- | --- |");
            foreach (var pin in ser.Pins)
            {
               sb.AppendLine($"| {pin.Name} | {pin.PinNumber} |");
            }
            sb.AppendLine();
            if (ser.Components.Any(comp => comp.Address is null))
            {
               sb.AppendLine("### Select Pins");
               sb.AppendLine();
               sb.AppendLine("| Name | Pin |");
               sb.AppendLine("| --- | --- |");
            }
            foreach (var comp in ser.Components)
            {
               if (comp.Address is null)
               {
                  sb.AppendLine($"| {comp.Name}_{comp.SelectPin.Name} | {comp.SelectPin.PinNumber} |");
               }
            }
            sb.AppendLine();
            if (ser.Components.Any(comp => comp.Address is not null))
            {
               sb.AppendLine("### Addresses");
               sb.AppendLine();
               sb.AppendLine("| Name | Pin |");
               sb.AppendLine("| --- | --- |");
            }
            foreach (var comp in ser.Components)
            {
               if (comp.Address is not null)
               {
                  sb.AppendLine($"| {comp.Name} | {comp.Address} |");
               }
            }
            sb.AppendLine();
         }
         sb.AppendLine();
      }

      private static void BuildFooter(StringBuilder sb)
      {
         sb.AppendLine();
         sb.AppendLine("> Auto-generated by [PinGenerator app.](https://github.com/Daxxn/PinGenerator)");
         sb.AppendLine(">");
         sb.AppendLine("> Author: Daxxn");
         sb.AppendLine();
      }

      private static void SaveFile(string path, string data)
      {
         using StreamWriter writer = new(path);
         writer.Write(data);
         writer.Flush();
      }
      #endregion
   }
}
