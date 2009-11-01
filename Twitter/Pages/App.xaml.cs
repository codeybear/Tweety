using System.Windows;
using System;

namespace Pages {
    public partial class App : Application {
        public void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) {
            MessageBox.Show(e.Exception.InnerException.Message);

            e.Handled = true;
        }

        private void Application_Startup(object sender, StartupEventArgs e) {

        }
    }
}
