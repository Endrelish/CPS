using System;
using System.Collections.Generic;
using System.Numerics;
using CPS1.Model.SignalData;

namespace CPS1.Model.Transform.FourierTransform
{
    public sealed class DiscreteFourierTransform : FourierTransform
    {
        protected override Complex TransformValue(int m, int N, Point[] signal)
        {
            var value = new Complex(0, 0);
            for (var i = 0; i < N; i++)
            {
                double exponent = 2 * Math.PI * m * i / N;
                value += new Complex(signal[i].Y, signal[i].Z) * Complex.Exp(new Complex(0, -exponent));
            }

            value /= N;
            return value;
        }

        public static Complex[] computeDft(Complex[] input)
        {
            int n = input.Length;
            Complex[] output = new Complex[n];
            for (int k = 0; k < n; k++)
            {  // For each output element
                Complex sum = 0;
                for (int t = 0; t < n; t++)
                {  // For each input element
                    double angle = 2 * Math.PI * t * k / n;
                    sum += input[t] * Complex.Exp(new Complex(0, -angle));
                }
                output[k] = sum;
            }
            return output;
        }

        protected override Complex ReverseTransformValue(int n, int N, List<Point> transform)
        {
            var value = new Complex(0,0);
            for (int i = 0; i < N; i++)
            {
                value += transform[i].Y * Complex.Exp(new Complex(0, 2 * Math.PI * n * i / N));
            }

            return value;
        }
    }
}