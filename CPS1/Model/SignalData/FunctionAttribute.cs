using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using CPS1.Properties;

namespace CPS1.Model.SignalData
{
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

        public FunctionAttribute(T value, bool visibility, T minValue, T maxValue, string name)
        {
            Value = value;
            Visibility = visibility;
            MinValue = minValue;
            MaxValue = maxValue;
            Name = name;
        }

        [DataMember]
        public T MaxValue
        {
            get => maxValue;
            set
            {
                if (value.Equals(maxValue))
                {
                    return;
                }

                maxValue = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public T MinValue
        {
            get => minValue;
            set
            {
                if (value.Equals(minValue))
                {
                    return;
                }

                minValue = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public string Name
        {
            get => name;
            set
            {
                if (value == name)
                {
                    return;
                }

                name = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public T Value
        {
            get => value;
            set
            {
                if (value.Equals(this.value))
                {
                    return;
                }

                this.value = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public bool Visibility
        {
            get => visibility;
            set
            {
                if (value == visibility)
                {
                    return;
                }

                visibility = value;
                OnPropertyChanged();
            }
        }

        [field: NonSerialized] public event PropertyChangedEventHandler PropertyChanged;

        public void AssignAttribute(FunctionAttribute<T> attribute)
        {
            MaxValue = attribute.MaxValue;
            MinValue = attribute.MinValue;
            Name = attribute.Name;
            Visibility = attribute.Visibility;
            Value = attribute.Value;
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}