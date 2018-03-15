namespace CPS1.Functions
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using CPS1.Annotations;

    public class FunctionAttribute<T> : INotifyPropertyChanged
        where T : struct
    {
        private T maxValue;

        private T minValue;

        private string name;

        private T value;

        private bool visibility;

        public FunctionAttribute(T value, bool visibility, T minValue, T maxValue, string name)
        {
            this.Value = value;
            this.Visibility = visibility;
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.Name = name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public T MaxValue
        {
            get => this.maxValue;
            set
            {
                if (value.Equals(this.maxValue))
                {
                    return;
                }

                this.maxValue = value;
                this.OnPropertyChanged();
            }
        }

        public T MinValue
        {
            get => this.minValue;
            set
            {
                if (value.Equals(this.minValue))
                {
                    return;
                }

                this.minValue = value;
                this.OnPropertyChanged();
            }
        }

        public string Name
        {
            get => this.name;
            set
            {
                if (value == this.name)
                {
                    return;
                }

                this.name = value;
                this.OnPropertyChanged();
            }
        }

        public T Value
        {
            get => this.value;
            set
            {
                if (value.Equals(this.value))
                {
                    return;
                }

                this.value = value;
                this.OnPropertyChanged();
            }
        }

        public bool Visibility
        {
            get => this.visibility;
            set
            {
                if (value == this.visibility)
                {
                    return;
                }

                this.visibility = value;
                this.OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}