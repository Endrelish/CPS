namespace CPS1.Model
{
    public interface IFileSerializer
    {
        string Format { get; }

        FunctionData Deserialize(string filename);

        void Serialize(FunctionData data, string filename);
    }
}