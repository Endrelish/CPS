namespace CPS1
{
    using System.IO;

    public interface IFileSerializer
    {
        void Serialize(object data, string filename);

        object Deserialize(string filename);
    }
}