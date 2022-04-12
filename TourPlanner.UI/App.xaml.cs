using Autofac;
using System.Windows;
using TourPlanner.BL.TourService;
using TourPlanner.DAL.Repositories;
using TourPlanner.UI.ViewModels;
using TourPlanner.UI.Views;

namespace TourPlanner.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        // Constructor Injection:
        private void OnStartup(object sender, StartupEventArgs e)
        {
            
            var mainViewModel = new MainViewModel(new TourService(new TourRepository()));

            var mainWindow = new MainWindow();

            mainWindow.DataContext = mainViewModel;

            mainWindow.Show();
        }
        
    }
}
