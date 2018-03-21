namespace CPS1.Model.Functions
{
    using System;

    public class UnitStepWave : Function
    {
        public UnitStepWave()
        {
            this.SignalFunction = (A, T, t1, kw, t) =>
                {
                    if (t < t1)
                    {
                        return 0d;
                    }

                    if (Math.Abs(t - t1) < double.Epsilon)
                    {
                        return A * 0.5d;
                    }

                    return A;
                };
        }

        public static Required RequiredAttributes { get; set; } =
            new Required(true, true, false, true, false, true, true, false);
    }
}