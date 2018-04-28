namespace CPS1.Model.SignalData
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    [Serializable]
    public class Point : IComparable
    {
        public Point()
        {
        }
        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        [DataMember]
        public double X { get; set; }
        [DataMember]
        public double Y { get; set; }

        public int CompareTo(object obj)
        {
                return this.X.CompareTo(((Point)obj).X);
        }

        public override string ToString()
        {
            return this.X + " | " + this.Y;
        }
    }
}