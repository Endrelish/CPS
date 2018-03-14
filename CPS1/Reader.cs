namespace CPS1
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;

    public static class Reader
    {
        public static IEnumerable<Point> ReadData(string filepath)
        {
            var pointsList = new List<Point>();
            try
            {
                using (var reader = new StreamReader(filepath))
                {
                    while (!reader.EndOfStream)
                    {
                        GetPoints(out double x, out double y, reader.ReadLine());
                        pointsList.Add(new Point(x, y));
                    }
                }
            }
            catch (IOException exception)
            {
                // TODO Handle exception
            }

            return pointsList;
        }

        private static void GetPoints(out double x, out double y, string line)
        {
            x = y = 0;
            try
            {
                x = double.Parse(line.Substring(0, line.IndexOf(' ') + 1));
                line = line.Substring(line.IndexOf(' ') + 1);
                y = double.Parse(line);
            }
            catch (Exception exception)
            {
                // TODO Handle exception
            }
        }
    }
}