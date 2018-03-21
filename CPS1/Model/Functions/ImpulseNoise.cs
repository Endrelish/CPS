namespace CPS1.Model.Functions
{
    using System;

    public class ImpulseNoise : Function
    {
        public ImpulseNoise()
        {
            this.SignalFunction = (A, T, t, t1, p) =>
                {
                    var random = new Random();
                    var threshold = random.NextDouble();
                    if (p <= threshold)
                    {
                        return A;
                    }

                    return 0;
                };
        }

        public static Required RequiredAttributes { get; } =
            new Required(true, true, false, true, false, true, false, true);
        
    }
}