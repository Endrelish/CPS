namespace CPS1.Model
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class SerializationFunctionHolder
    {
        public SerializationFunctionHolder(Func<FunctionData, double, double> function, FunctionData data)
        {
            Data = data;
            Function = function;
        }
        [DataMember]
        public Func<FunctionData, double, double> Function { get; set; }
        [DataMember]
        public FunctionData Data { get; set; }
    }
}