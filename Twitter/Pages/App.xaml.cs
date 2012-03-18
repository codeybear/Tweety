using System.Windows;
using Tweety.Core;

namespace Pages {
    public partial class App : Application {
        App() {
            IOCSetup.Setup();
        }
    }
}
