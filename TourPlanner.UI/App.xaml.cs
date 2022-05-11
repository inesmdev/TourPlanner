using Autofac;
using System.Windows;
using TourPlanner.DAL.Repositories;
using TourPlanner.UI.ViewModels;
using TourPlanner.UI.Views;
using Serilog;
using TourPlanner.UI.Logging;

namespace TourPlanner.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //private static ILoggerWrapper logger = LoggerFactory.GetLogger();
        
        // Constructor Injection:
        private void OnStartup(object sender, StartupEventArgs e)
        {
            // Setup Serilog:
           
            /*Log.Logger = new LoggerConfiguration()
                          .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                          .MinimumLevel.Verbose()
                          .CreateLogger();*/

            Log.Logger.Debug("Programm started");
            

            var mainViewModel = new MainViewModel();

            var mainWindow = new MainWindow();

            mainWindow.DataContext = mainViewModel;

            // Emit a Startup Event -> , Command nur nur button click sondern auch iw bei start

            mainWindow.Show();



        }
    }
}
