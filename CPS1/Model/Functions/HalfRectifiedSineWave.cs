namespace CPS1.Model.Functions
{
    using System;

    public class HalfRectifiedSineWave : Function
    {
        public HalfRectifiedSineWave()
        {
            this.SignalFunction = (A, T, t, t1, p) =>
                {
                    var ret = A * Math.Sin(Math.PI * 2 * (t - t1) / T);
                    if (ret.CompareTo(0) < 0)
                    {
                        ret = 0;
                    }

                    return ret;
                };
        }

        public static Required RequiredAttributes { get; } = new Required(true, true, true, true, false, true,true, false);
    }
}