using System.ComponentModel;

namespace CPS1.Model.SignalData
{
    public interface IFunctionAttribute : INotifyPropertyChanged
    {
        string Name { get; set; }

        bool Visibility { get; set; }
    }
}