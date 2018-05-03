using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using CPS1.Annotations;
using CPS1.Properties;

namespace CPS1.Model.Parameters
{
    [DataContract]
    [Serializable]
    public class Parameter : INotifyPropertyChanged
    {
        [DataMember]
        private string name;

        [DataMember]
        private double value;

        public Parameter(double value, string name)
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

        [field:NonSerialized]
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