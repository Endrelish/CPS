using System.IO;
using System.Runtime.Serialization;
using CPS1.Model.SignalData;

namespace CPS1.Model.Serialization
{
    public class FileXmlSerializer : IFileSerializer
    {
        private readonly DataContractSerializer serializer;

        public FileXmlSerializer()
        {
            serializer = new DataContractSerializer(typeof(FunctionData));
            Format = "XML files (*.xml)|*.xml";
        }

        public string Format { get; }

        public FunctionData Deserialize(string filename)
        {
            FunctionData data;
            using (var stream = new FileStream(filename, FileMode.Open))
            {
                data = (FunctionData) serializer.ReadObject(stream);
            }

            return data;
        }

        public void Serialize(FunctionData data, string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Create))
            {
                serializer.WriteObject(stream, data);
            }
        }
    }
}