using System;
using System.Collections.Generic;
using CPS1.Model.Exceptions;
using CPS1.Model.SignalData;
using Microsoft.CSharp.RuntimeBinder;

namespace CPS1.Model.Transform
{
    public class Matrix<T> where T : struct
    {
        private readonly T[,] values;


        public Matrix(int sizeX, int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            values = new T[sizeX, sizeY];
        }

        public int SizeX { get; set; }
        public int SizeY { get; set; }

        public T this[int x, int y]
        {
            get => values[x, y];
            set => values[x, y] = value;
        }

        public void ForEach(Func<T, T> func)
        {
            for (var i = 0; i < SizeX; i++)
            {
                for (var j = 0; j < SizeY; j++)
                {
                    values[i, j] = func(values[i, j]);
                }
            }
        }

        public Matrix<T> Clone()
        {
            var returnMatrix = new Matrix<T>(SizeX, SizeY);
            for (var i = 0; i < SizeX; i++)
            {
                for (var j = 0; j < SizeY; j++)
                {
                    returnMatrix[i, j] = this[i, j];
                }
            }

            return returnMatrix;
        }

        public static Matrix<T> operator *(T factor, Matrix<T> m)
        {
            return m * factor;
        }
        public static Matrix<T> operator *(Matrix<T> first, T factor)
        {
            var ret = first.Clone();
            ret.ForEach(t => (dynamic) factor * (dynamic) t);
            return ret;
        }

        public static Matrix<T> operator *(Matrix<T> first, Matrix<T> second)
        {
            if (first.SizeX != second.SizeY)
            {
                throw new MatrixOperationException("Matrices sizes are not equal.");
            }

            var matrix = new Matrix<T>(second.SizeX, first.SizeY);

            for (var i = 0; i < second.SizeX; i++)
            {
                for (var j = 0; j < first.SizeY; j++)
                {
                    T sum = (dynamic) 0;
                    for (var k = 0; k < first.SizeX; k++)
                    {
                        try
                        {
                            sum += (dynamic) first[k, j] * (dynamic) second[i, k];
                        }
                        catch (RuntimeBinderException e)
                        {
                            throw new MatrixOperationException("Cannot multiply matrices of " + typeof(T), e);
                        }
                    }

                    matrix[i, j] = sum;
                }
            }

            return matrix;
        }

        public List<T> ToList()
        {
            var list = new List<T>();
            int index = 0;
            foreach (var value in values)
            {
                list.Add(value);
            }

            return list;
        }
    }
}