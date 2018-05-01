using System.Windows;
using System.Windows.Threading;

namespace CPS1
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("You shouldn't have done that, look what happened: " + e.Exception.Message,
                "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Warning);
            e.Handled = true;
        }
    }
}