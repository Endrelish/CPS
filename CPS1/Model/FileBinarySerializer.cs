namespace CPS1.Model
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    public class FileBinarySerializer : IFileSerializer
    {
        private readonly BinaryFormatter formatter;

        public string Format { get; }

        public FileBinarySerializer()
        {
            this.formatter = new BinaryFormatter();
            Format = "Binary files (*.bin)|*.bin";
        }

        public FunctionData Deserialize(string filename)
        {
            SerializationFunctionHolder deserializedData;
            using (var stream = new FileStream(filename, FileMode.Open))
            {
                deserializedData = (SerializationFunctionHolder)this.formatter.Deserialize(stream);
            }

            FunctionData data = deserializedData.Data;
            data.Function = deserializedData.Function;
            return data;
        }

        public void Serialize(FunctionData data, string filename)
        {
            var dataToSerialize = new SerializationFunctionHolder(data.Function, data);
            using (var stream = new FileStream(filename, FileMode.Create))
            {
                this.formatter.Serialize(stream, dataToSerialize);
            }
        }
    }
}