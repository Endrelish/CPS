using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CPS1.Model.Generation;
using CPS1.Model.SignalData;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace CPS1.View
{
    /// <summary>
    /// Interaction logic for ChartsTest.xaml
    /// </summary>
    public partial class ChartsTest : UserControl
    {
        private double _lastLecture;
        private double _trend;
        public ChartsTest()
        {
            InitializeComponent();
            
            ThreadStart childref = new ThreadStart(Function);
            Thread childThread = new Thread(childref);
            childThread.Start();

            DataContext = this;
        }

        public void Function()
        {
            var fun = new FunctionData();
            fun.Period.Value = Math.PI;
            Values = new ChartValues<ObservableValue>();
            for (int i = 0; i < 300; i++)
            {
                Values.Add(new ObservableValue(fun.Function(fun, 0.1 * i)));
            }

            var currT = 29.9d;
            //Task.Run(() =>
            //{
                while (true)
                {
                    Thread.Sleep(500);
                    currT += 0.1;
                    //Application.Current.Dispatcher.Invoke(() =>
                    //{
                    Values.Add(new ObservableValue(fun.Function(fun, currT)));
                    Values.RemoveAt(0);
                    //});
                }
            //});
        }

        public ChartValues<ObservableValue> Values { get; set; }

        public double LastLecture
        {
            get { return _lastLecture; }
            set
            {
                _lastLecture = value;
                OnPropertyChanged("LastLecture");
            }
        }

        private void SetLecture()
        {
            var target = ((ChartValues<ObservableValue>)Values).Last().Value;
            var step = (target - _lastLecture) / 4;
            Task.Run(() =>
            {
                for (var i = 0; i < 4; i++)
                {
                    Thread.Sleep(100);
                    LastLecture += step;
                }
                LastLecture = target;
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
