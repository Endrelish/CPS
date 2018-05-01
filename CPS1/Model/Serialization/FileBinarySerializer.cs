using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CPS1.Model.SignalData;

namespace CPS1.Model.Serialization
{
    public class FileBinarySerializer : IFileSerializer
    {
        private readonly BinaryFormatter formatter;

        public FileBinarySerializer()
        {
            formatter = new BinaryFormatter();
            Format = "Binary files (*.bin)|*.bin";
        }

        public string Format { get; }

        public FunctionData Deserialize(string filename)
        {
            FunctionData data;
            using (var stream = new FileStream(filename, FileMode.Open))
            {
                data = (FunctionData) formatter.Deserialize(stream);
            }

            return data;
        }

        public void Serialize(FunctionData data, string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Create))
            {
                formatter.Serialize(stream, data);
            }
        }
    }
}