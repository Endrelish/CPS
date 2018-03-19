namespace CPS1.Functions
{
    using System;

    public class SymmetricalSquareWave : Function
    {
        public SymmetricalSquareWave()
        {
            this.SignalFunction = (A, T, t1, kw, t) =>
                {
                    var k = (int)Math.Floor((t - t1) / T);
                    var result = t - t1 - k * T;
                    if (result < kw * T)
                    {
                        return A;
                    }

                    return -A;
                };
        }

        public static Required RequiredAttributes { get; set; } = new Required(true, true, true, true, true, true, true,false);
    }
}