namespace CPS1.Functions
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class FunctionAttribute<T> : INotifyPropertyChanged where T : struct
    {
        private T value;

        private bool visibility;

        private T minValue;

        private T maxValue;

        public T Value
        {
            get => this.value;
            set
            {
                this.value = value;
                this.OnPropertyChanged("Value");
            }
        }

        public bool Visibility
        {
            get => this.visibility;
            set
            {
                this.visibility = value;
                this.OnPropertyChanged("Visibility");
            }
        }

        public T MinValue
        {
            get => this.minValue;
            set
            {
                this.minValue = value;
                this.OnPropertyChanged("Value");
            }
        }

        public T MaxValue
        {
            get => this.maxValue;
            set
            {
                this.maxValue = value;
                this.OnPropertyChanged("Value");
            }
        }

        public FunctionAttribute(T value, bool visibility, T minValue, T maxValue)
        {
            this.Value = value;
            this.Visibility = visibility;
            this.MinValue = minValue;
            this.MaxValue = maxValue;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}