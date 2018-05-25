using System;
using System.Runtime.Serialization;

namespace CPS1.Model.SignalData
{
    [DataContract]
    [Serializable]
    public class Point : IComparable
    {
        public Point()
        {
        }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
        public Point(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        [DataMember] public double X { get; set; }

        [DataMember] public double Y { get; set; }
        [DataMember] public double Z { get; set; }

        public int CompareTo(object obj)
        {
            return X.CompareTo(((Point) obj).X);
        }

        public override string ToString()
        {
            return X + " | " + Y;
        }
    }
}