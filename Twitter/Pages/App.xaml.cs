using System.Windows;
using System;

namespace Pages {
    public partial class App : Application {
        public void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) {
            e.Handled = true;
            Pages.MainWindow.ShowError(e.Exception.InnerException.Message);
        }
    }
}
