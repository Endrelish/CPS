namespace CPS1.Model.SignalData
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;

    using CPS1.Properties;

    [DataContract]
    [Serializable]
    public class FunctionAttribute<T> : INotifyPropertyChanged
        where T : struct
    {
        private T maxValue;

        private T minValue;

        private string name;

        private T value;

        private bool visibility;

        public FunctionAttribute()
        {
        }

        public FunctionAttribute(T value, bool visibility, T minValue, T maxValue, string name)
        {
            this.Value = value;
            this.Visibility = visibility;
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.Name = name;
        }
        [field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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
        
        public void AssignAttribute(FunctionAttribute<T> attribute)
        {
            this.MaxValue = attribute.MaxValue;
            this.MinValue = attribute.MinValue;
            this.Name = attribute.Name;
            this.Visibility = attribute.Visibility;
            this.Value = attribute.Value;
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}