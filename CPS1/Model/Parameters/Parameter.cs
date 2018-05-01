using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CPS1.Properties;

namespace CPS1.Model.Parameters
{
    public class Parameter : INotifyPropertyChanged
    {
        private string name;

        private double value;

        public Parameter(double value, string name, bool visibility = false)
        {
            Value = value;
            Name = name;
        }

        public string Name
        {
            get => name;
            set
            {
                if (name == value)
                {
                    return;
                }

                name = value;
                OnPropertyChanged();
            }
        }

        public double Value
        {
            get => value;
            set
            {
                if (Math.Abs(this.value - value) < double.Epsilon)
                {
                    return;
                }

                this.value = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void AssingParameter(Parameter p)
        {
            Value = p.value;
            Name = p.Name;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}