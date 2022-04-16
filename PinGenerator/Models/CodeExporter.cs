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
         if (!string.IsNullOrEmpty(proj.ExportPath))
         {
            StringBuilder sb = BuildHeader();
            BuildCompContainer(sb, proj.Micro.DigitalComponents, true);
            BuildCompContainer(sb, proj.Micro.AnalogComponents, false);
            BuildFooter(sb);

            SaveFile(proj.ExportPath, sb.ToString());
         }
      }

      private static StringBuilder BuildHeader()
      {
         var sb = new StringBuilder("// Auto-Generated code file from PinGenerator.");
         sb.AppendLine("// Avoid editing this file.");
         sb.AppendLine();
         sb.AppendLine("#pragma once");
         sb.AppendLine("#include <Arduino.h>");
         sb.AppendLine();
         return sb;
      }

      private static void BuildFooter(StringBuilder sb)
      {
         sb.AppendLine();
         sb.AppendLine($"Author: Daxxn");
         sb.AppendLine("// Auto-Generated code file from PinGenerator.");
         sb.AppendLine("// Avoid editing this file.");
         sb.AppendLine();
      }

      private static void BuildCompContainer(StringBuilder sb, IEnumerable<Component> comps, bool isDigital)
      {
         sb.AppendLine(isDigital ? "namespace D {" : "namespace A {");

         foreach (var comp in comps)
         {
            BuildComponent(sb, comp);
         }
         sb.AppendLine("}");
      }

      private static void BuildComponent(StringBuilder sb, Component comp)
      {
         sb.AppendLine($"\t{comp.Export()}");
         foreach (var pin in comp.Pins)
         {
            sb.AppendLine($"\t{pin.Export()}");
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
