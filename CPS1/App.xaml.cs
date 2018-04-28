namespace CPS1
{
    using System.Windows;

    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("You shouldn't have done that, look what happened: " + e.Exception.Message, "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Warning);
            e.Handled = true;
        }
    }
}