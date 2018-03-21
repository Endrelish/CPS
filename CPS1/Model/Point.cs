namespace CPS1.Model
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class Point
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
    }
}