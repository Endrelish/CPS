namespace CPS1.Model.SignalData
{
    using System.ComponentModel;

    public interface IFunctionAttribute : INotifyPropertyChanged
    {
        string Name { get; set; }

        bool Visibility { get; set; }
    }
}