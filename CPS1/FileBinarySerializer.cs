namespace CPS1
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    public class FileBinarySerializer : IFileSerializer
    {
        private readonly BinaryFormatter formatter;

        public FileBinarySerializer()
        {
            this.formatter = new BinaryFormatter();
        }

        public object Deserialize(string filename)
        {
            object data;
            using (var stream = new FileStream(filename, FileMode.Open))
            {
                data = this.formatter.Deserialize(stream);
            }

            return data;
        }

        public void Serialize(object data, string filename)
        {
            using (var stream = new FileStream(filename, FileMode.OpenOrCreate))
            {
                this.formatter.Serialize(stream, data);
            }
        }
    }
}