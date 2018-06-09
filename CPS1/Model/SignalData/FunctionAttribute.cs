using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using CPS1.Annotations;
using CPS1.Properties;

namespace CPS1.Model.SignalData
{
    [DataContract]
    [Serializable]
    public class FunctionAttribute<T> : INotifyPropertyChanged
        where T : struct
    {
        private T _maxValue;

        private T _minValue;

        private string _name;

        private T _value;

        private bool _visibility;

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
            get => _maxValue;
            set
            {
                if (value.Equals(_maxValue))
                {
                    return;
                }

                _maxValue = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public T MinValue
        {
            get => _minValue;
            set
            {
                if (value.Equals(_minValue))
                {
                    return;
                }

                _minValue = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public string Name
        {
            get => _name;
            set
            {
                if (value == _name)
                {
                    return;
                }

                _name = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public T Value
        {
            get => _value;
            set
            {
                if (value.Equals(this._value))
                {
                    return;
                }

                this._value = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public bool Visibility
        {
            get => _visibility;
            set
            {
                if (value == _visibility)
                {
                    return;
                }

                _visibility = value;
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