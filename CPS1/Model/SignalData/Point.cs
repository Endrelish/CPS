using System;
using System.Collections;
using System.Numerics;
using System.Runtime.Serialization;

namespace CPS1.Model.SignalData
{
    [DataContract]
    [Serializable]
    public class Point : IComparable, IEquatable<Point>
    {
        public Point()
        {
        }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point(double x, Complex yz)
        {
            X = x;
            Y = yz.Real;
            Z = yz.Imaginary;
        }
        public Point(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void Round(double tolerance)
        {
            if (Y - Math.Floor(Y) < tolerance) Y = Math.Floor(Y);
            else if (Math.Ceiling(Y) - Y < tolerance) Y = Math.Ceiling(Y);
            if (Z - Math.Floor(Z) < tolerance) Z = Math.Floor(Z);
            else if (Math.Ceiling(Z) - Z < tolerance) Z = Math.Ceiling(Z);
        }

        public Complex ToComplex()
        {
            return new Complex(Y, Z);
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
            return X + " | " + Y + " | " + Z;
        }

        public new bool Equals(object x, object y)
        {
            if (x is Point f && y is Point s)
            {
                return Math.Abs(f.X - s.X) < Double.Epsilon && Math.Abs(f.Y - s.Y) < double.Epsilon && Math.Abs(f.Z - s.Z) < double.Epsilon;
            }

            return false;
        }

        public int GetHashCode(object obj)
        {
            return base.GetHashCode();
        }

        public bool Equals(Point other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Point) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                return hashCode;
            }
        }
    }
}