using System.Windows;
using System.Windows.Threading;
using CPS1.Model.Exceptions;

namespace CPS1
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (e.Exception.GetType() == typeof(InvalidSamplesNumberException))
            {
                MessageBox.Show(e.Exception.Message, "It was a mistake", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("You shouldn't have done that, look what happened: " + e.Exception.Message,
                    "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            e.Handled = true;
        }
    }
}