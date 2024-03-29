﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CPS1.Model.Exceptions;
using CPS1.Model.SignalData;

namespace CPS1.Model.Transform.FourierTransform
{
    public abstract class FourierTransform : ITransform
    {
        protected FourierTransform(string name)
        {
            Name = name;
        }

        public virtual IEnumerable<Point> Transform(Point[] signal)
        {
            var N = signal.Length;
            if((N != 0) && ((N & (N - 1)) != 0)) throw new InvalidSamplesNumberException("The number of samples must be a power of 2.");
            var transform = new Point[N];


            for (var i = 0; i < N; i++)
            {
                transform[i] = new Point(i, TransformValue(i, N, signal));
                transform[i].Round(10E-15);
            }

            var duration = (N - 1) * (signal[1].X - signal[0].X);

            foreach (var point in transform)
            {
                point.X /= duration;
            }

            return transform;
        }

        public string Name { get; }

        protected Complex TwiddleFactor(int k, int m, int N)
        {
            return Complex.Exp(new Complex(0, -2 * Math.PI * m * k / N));
        }

        protected abstract Complex TransformValue(int m, int N, Point[] signal);
        protected abstract Complex ReverseTransformValue(int m, int N, List<Point> signal);

        public sealed override string ToString()
        {
            return Name;
        }
    }
}