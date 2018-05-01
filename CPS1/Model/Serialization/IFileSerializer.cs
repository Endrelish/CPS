using CPS1.Model.SignalData;

namespace CPS1.Model.Serialization
{
    public interface IFileSerializer
    {
        string Format { get; }
        void Serialize(FunctionData data, string filename);

        FunctionData Deserialize(string filename);
    }
}