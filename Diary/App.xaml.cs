using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Windows;
using Diary.Views;

namespace Diary
{
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var metroWindow = Current.MainWindow as MetroWindow;
            metroWindow.ShowMessageAsync("Nieoczekiwany  wyjątek",
                "Wystąpił nieoczekiwany wyjątek." +
                Environment.NewLine +
                e.Exception.Message);

            e.Handled = true;
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Diary.Views.SplashScreen splashScreen = new Diary.Views.SplashScreen();
            splashScreen.Show();
            MainWindow mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
            splashScreen.Close();
        }
    }
}
