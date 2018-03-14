namespace CPS1
{
    using System.Windows.Controls;

    /// <summary>
    ///     Interaction logic for SignalChart.xaml
    /// </summary>
    public partial class SignalChart : UserControl
    {
        public SignalChart()
        {
            this.InitializeComponent();
        }

        public int ChartNumber { get; set; }
    }
}