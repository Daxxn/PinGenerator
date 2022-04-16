using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinGenerator.Models
{
   public static class Const
   {
      public static readonly List<FileDialogCustomPlace> CustomProjectDirs = new()
      {
         new(@$"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\PinGenerator\Projects")
      };

      public static readonly List<FileDialogCustomPlace> CustomExportDirs = new()
      {
         new($@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\Code\Arduino"),
         new($@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\Code\C++")
      };
   }
}
