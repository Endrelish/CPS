namespace CPS1.Model.Functions
{
    using System;

    public class RandomNoiseWave : Function
    {
        public RandomNoiseWave()
        {
            this.SignalFunction = (A, T, t, t1, p) =>
                {
                    var random = new Random();
                    var y = random.NextDouble() * (A * 2) - A;

                    return y;
                };
        }

        public static Required RequiredAttributes { get; } =
            new Required(true, true, false, true, false, true, false, false);
    }
}