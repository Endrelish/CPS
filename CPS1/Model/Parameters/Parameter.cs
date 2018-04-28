namespace CPS1.Model.Parameters
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using CPS1.Properties;

    public class Parameter : INotifyPropertyChanged
    {   
        private string name;

        private double value;

        public Parameter(double value, string name, bool visibility = false)
        {
            this.Value = value;
            this.Name = name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get => this.name;
            set
            {
                if (this.name == value)
                {
                    return;
                }

                this.name = value;
                this.OnPropertyChanged();
            }
        }

        public double Value
        {
            get => this.value;
            set
            {
                if (Math.Abs(this.value - value) < double.Epsilon)
                {
                    return;
                }

                this.value = value;
                this.OnPropertyChanged();
            }
        }

        public void AssingParameter(Parameter p)
        {
            this.Value = p.value;
            this.Name = p.Name;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}