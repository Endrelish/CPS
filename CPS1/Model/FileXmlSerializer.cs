namespace CPS1.Model
{
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;

    public class FileXmlSerializer : IFileSerializer
    {
        private readonly DataContractSerializer serializer;

        public FileXmlSerializer()
        {
            this.serializer = new DataContractSerializer(typeof(FunctionData));
            this.Format = "XML files (*.xml)|*.xml";
        }

        public string Format { get; }

        public FunctionData Deserialize(string filename)
        {
            FunctionData data;
            using (var stream = new FileStream(filename, FileMode.Open))
            {
                data = (FunctionData)this.serializer.ReadObject(stream);
            }

            return data;
        }

        public void Serialize(FunctionData data, string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Create))
            {
                this.serializer.WriteObject(stream, data);
            }
        }
    }
}