namespace CPS1
{
    using CPS1.Functions;

    public static class Generator
    {
        static Generator()
        {
            FullyRectifiedSine = new FullyRectifiedSineWave();
            HalfRectifiedSine = new HalfRectifiedSineWave();
            NormalDistribution = new NormalDistributionWave();
            RandomNoise = new RandomNoiseWave();
            Sine = new SineWave();
            Square = new SquareWave();
            SymmetricalSquare = new SymmetricalSquareWave();
            Triangle = new TriangleWave();
        }

        private static IFunction FullyRectifiedSine { get; }

        private static IFunction HalfRectifiedSine { get; }

        private static IFunction NormalDistribution { get; }

        private static IFunction RandomNoise { get; }

        private static IFunction Sine { get; }

        private static IFunction Square { get; }

        private static IFunction SymmetricalSquare { get; }

        private static IFunction Triangle { get; }

        public static void GenerateSignal(FunctionData data, Signal signal)
        {
            switch (signal)
            {
                case Signal.FullyRectifiedSine:
                    FullyRectifiedSine.GeneratePoints(data);
                    break;
                case Signal.HalfRectifiedSine:
                    HalfRectifiedSine.GeneratePoints(data);
                    break;
                case Signal.NormalDistribution:
                    NormalDistribution.GeneratePoints(data);
                    break;
                case Signal.RandomNoise:
                    RandomNoise.GeneratePoints(data);
                    break;
                case Signal.Sine:
                    Sine.GeneratePoints(data);
                    break;
                case Signal.Square:
                    Square.GeneratePoints(data);
                    break;
                case Signal.SymmetricalSquare:
                    SymmetricalSquare.GeneratePoints(data);
                    break;
                case Signal.Triangle:
                    Triangle.GeneratePoints(data);
                    break;
            }
        }
    }
}