using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinGenerator.Models
{
   public static class CodeExporter
   {
      #region Local Props
      #endregion

      #region Methods
      public static void Export(Project proj)
      {
         if (!string.IsNullOrEmpty(proj.ExportCodePath))
         {
            StringBuilder sb = BuildHeader();
            BuildDigitalCompContainer(sb, proj.Micro.DigitalComponents);
            BuildAnalogCompContainer(sb, proj.Micro.AnalogComponents);
            BuildSerialCompContainer(sb, proj.Micro.Serial);
            BuildFooter(sb);

            SaveFile(proj.ExportCodePath, sb.ToString());
         }
      }

      public static async Task ExportAsync(Project project)
      {
         await Task.Run(() =>
         {
            Export(project);
         });
      }

      private static StringBuilder BuildHeader()
      {
         var sb = new StringBuilder("// Auto-Generated code file from PinGenerator.");
         sb.AppendLine("\n// Avoid editing this file.");
         sb.AppendLine();
         sb.AppendLine("#pragma once");
         sb.AppendLine("#include <Arduino.h>");
         sb.AppendLine();
         return sb;
      }

      private static void BuildFooter(StringBuilder sb)
      {
         sb.AppendLine();
         sb.AppendLine("// Author: Daxxn");
         sb.AppendLine("// Auto-Generated code file from PinGenerator.");
         sb.AppendLine("// Avoid editing this file.");
         sb.AppendLine();
      }

      private static void BuildAnalogCompContainer(StringBuilder sb, IEnumerable<Component> components)
      {
         sb.AppendLine("namespace Anlg {");

         foreach (var comp in components)
         {
            BuildComponent(sb, comp, false);
         }
         sb.AppendLine("}");
      }

      private static void BuildDigitalCompContainer(StringBuilder sb, IEnumerable<Component> components)
      {
         sb.AppendLine("namespace Digitl {");

         foreach (var comp in components)
         {
            BuildComponent(sb, comp, true);
         }
         sb.AppendLine("}");
      }

      private static void BuildSerialCompContainer(StringBuilder sb, IEnumerable<Serial> serials)
      {
         sb.AppendLine("namespace Ser {");
         foreach (var serial in serials)
         {
            if (serial.Components.Count == 0) break;
            sb.AppendLine($"\tnamespace {serial.Name} {{");
            foreach (var pin in serial.Pins)
            {
               sb.AppendLine($"\t\tconst int {pin.Name} = {pin.PinNumber};");
            }


            if (serial.Components.Any(c => c.Address is null))
            {
               sb.AppendLine();
            }
            foreach (var comp in serial.Components)
            {
               if (comp.Address is null)
               {
                  sb.AppendLine($"\t\tconst int {comp.Name}_{comp.SelectPin?.Name} = {comp.SelectPin?.PinNumber};");
               }
            }

            if (serial.Components.Any(c => c.Address is not null))
            {
               sb.AppendLine();
            }
            foreach (var comp in serial.Components)
            {
               if (comp.Address is not null)
               {
                  sb.AppendLine($"\t\tconst int {comp.Name}_ADDR = {comp.Address};");
               }
            }
            sb.AppendLine("\t}");
         }
         sb.AppendLine("}");
      }

      private static void BuildComponent(StringBuilder sb, Component comp, bool isDigital)
      {
         sb.AppendLine($"\t{comp.Export()}");
         foreach (var pin in comp.Pins)
         {
            sb.AppendLine($"\t\tconst int {pin.Name}_PIN = {(!isDigital ? "A" : "")}{pin.PinNumber};");
         }
         sb.AppendLine("\t}");
      }

      private static void SaveFile(string path, string data)
      {
         using StreamWriter writer = new(path);
         writer.Write(data);
         writer.Flush();
      }
      #endregion

      #region Full Props

      #endregion
   }
}
