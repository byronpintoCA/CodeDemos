using ByronSouthParkDemo.Common;
using System.Windows;

namespace ByronSouthParkDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            SouthParkViewModelFactory.GetInstance().Stop();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SouthParkViewModelFactory.GetInstance().Start();
        }
    }
}
