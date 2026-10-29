using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using Microsoft.Shell;
using SafeNotePad.Logging;
using SafeNotePad.Utils;
using SafeNotePad.Views;

namespace SafeNotePad
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ISingleInstanceApp
    {
        public App()
        {
            Settings.ApplicationAssemblyName = Assembly.GetCallingAssembly().FullName;
            Dispatcher.UnhandledException += DispatcherOnUnhandledException;
        }

        private void DispatcherOnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMessage = string.Format("An unhandled exception occurred: {0}. Stack: {1}", e.Exception.Message, e.Exception.StackTrace);
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            var logger = LogManager.GetLogger("Application");
            logger.Error(e.Exception);

            e.Handled = true;
        }

        private const string Unique = "SafeNotePad.exe-34785638477756873465873777";

        [STAThread]
        public static void Main(string[] args)
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
            {
                StartApplication(args);

                // Allow single instance code to perform cleanup operations
                SingleInstance<App>.Cleanup();
            }
        }

        private static void StartApplication(IList<string> args)
        {
            var application = new App();
            application.InitializeComponent();
            application.Run(new MainWindow(args.FirstOrDefault()));
        }

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            if (MainWindow.Visibility == Visibility.Hidden || MainWindow.Visibility == Visibility.Collapsed)
            {
                MainWindow.Show();
            }

            // Bring window to foreground
            if (MainWindow.WindowState == WindowState.Minimized)
            {
                MainWindow.WindowState = WindowState.Normal;
            }

            if (args.Any())
            {
                ((MainWindow)MainWindow).ViewModel.LoadNote(args.Last());
            }

            MainWindow.Activate();

            return true;
        }
    }
}
