namespace CPS1
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;

    public static class Writer
    {
        public static void Write(IEnumerable<Point> points, string filepath)
        {
            try
            {
                using (var writer = new StreamWriter(filepath))
                {
                    foreach (var point in points)
                    {
                        writer.WriteLine("{0} {1}",point.X, point.Y);
                    }
                }
            }
            catch (Exception exception)
            {
                //TODO Handle exception
            }
        }
    }
}