namespace CPS1.Model.Serialization
{
    using CPS1.Model.SignalData;

    public interface IFileSerializer
    {
        void Serialize(FunctionData data, string filename);

        FunctionData Deserialize(string filename);
        string Format { get; }
    }
}