using System.Windows;
using Core;

namespace Pages {
    public partial class App : Application {
        App() {
            IOCSetup.Setup();
        }
    }
}
