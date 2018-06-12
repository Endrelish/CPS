using System;
using System.Collections.Generic;
using System.Linq;
using CPS1.Model.ConvolutionFiltrationCorrelation.Windows;
using CPS1.Model.SignalData;

namespace CPS1.Model.ConvolutionFiltrationCorrelation.Filters
{
    public class Filter
    {
        private readonly Dictionary<FilterType, Func<int, double>> filterTypes;

        public Filter()
        {
            filterTypes = new Dictionary<FilterType, Func<int, double>>();
            filterTypes.Add(FilterType.LowPassFilter, n => 1.0d);
            filterTypes.Add(FilterType.HighPassFilter, n => Math.Pow(-1, n));
        }

        public IEnumerable<Point> Filtration(int M, double fo, IEnumerable<Point> points, IWindow window,
            FilterType filterType)
        {
            var fp = 1 / (points.ElementAt(1).X - points.ElementAt(0).X);
            if (filterType == FilterType.HighPassFilter)
            {
                fo = fp / 2 - fo;
            }

            var response = new List<Point>();
            var K = fp / fo;
            for (var i = 0; i < M; i++)
            {
                response.Add(new Point(i,
                    ImpulseResponse.Response(i, K, M) * window.Window(i, M) * filterTypes[filterType](i)));
            }

            var ret = Convolution.Convolute(points, response);
            var amplitude = ret.Select(p => p.Y).Max() * 2;
            amplitude = points.Select(p => p.Y).Max() / amplitude;

            foreach (var point in ret)
            {
                point.Y *= amplitude;
            }

            return ret;
        }
    }
}