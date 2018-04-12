namespace CPS1.Model
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    public class FileBinarySerializer : IFileSerializer
    {
        private readonly BinaryFormatter formatter;

        public FileBinarySerializer()
        {
            this.formatter = new BinaryFormatter();
            this.Format = "Binary files (*.bin)|*.bin";
        }

        public string Format { get; }

        public FunctionData Deserialize(string filename)
        {
            FunctionData data;
            using (var stream = new FileStream(filename, FileMode.Open))
            {
                data = (FunctionData)this.formatter.Deserialize(stream);
            }

            return data;
        }

        public void Serialize(FunctionData data, string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Create))
            {
                this.formatter.Serialize(stream, data);
            }
        }
    }
}