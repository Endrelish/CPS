namespace CPS1.Model
{
    using System.Collections.Generic;

    public interface IFileSerializer
    {
        void Serialize(FunctionData data, string filename);

        FunctionData Deserialize(string filename);
        string Format { get; }
    }
}