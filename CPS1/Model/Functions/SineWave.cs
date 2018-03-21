namespace CPS1.Model.Functions
{
    using System;

    public class SineWave : Function
    {
        public SineWave()
        {
            this.SignalFunction = (A, T, t, t1, p) => A * Math.Sin(Math.PI * 2 * (t - t1) / T);
        }

        public static Required RequiredAttributes { get; } = new Required(true, true, true, true, false, true, true, false);
        
    }
}