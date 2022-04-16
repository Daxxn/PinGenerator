using PinGenerator.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PinGenerator
{
   /// <summary>
   /// Interaction logic for App.xaml
   /// </summary>
   public partial class App : Application
   {
      protected override void OnStartup(StartupEventArgs e)
      {
         MicroController.OnStart();
         base.OnStartup(e);
      }
      protected override void OnExit(ExitEventArgs e)
      {
         MicroController.OnExit();
         base.OnExit(e);
      }
   }
}
