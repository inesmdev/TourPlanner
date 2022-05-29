using System.Windows;
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
            var mainViewModel = new MainViewModel();

            var mainWindow = new MainWindow();

            mainWindow.DataContext = mainViewModel;

            mainWindow.Show();
        }
    }
}
