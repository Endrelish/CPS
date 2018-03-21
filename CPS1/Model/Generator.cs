namespace CPS1.Model
{
    using CPS1.Model.Functions;

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
            UnitStep = new UnitStepWave();
            KroneckerDelta = new KroneckerDelta();
        }

        private static Function FullyRectifiedSine { get; }

        private static Function HalfRectifiedSine { get; }

        private static Function NormalDistribution { get; }

        private static Function RandomNoise { get; }

        private static Function Sine { get; }

        private static Function Square { get; }

        private static Function SymmetricalSquare { get; }

        private static Function Triangle { get; }
        private static Function UnitStep { get; }
        private static Function KroneckerDelta { get; }

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
                case Signal.KroneckerDelta:
                    KroneckerDelta.GeneratePoints(data);
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
                case Signal.UnitStep:
                    UnitStep.GeneratePoints(data);
                    break;
            }
            data.PointsUpdate();
        }
    }
}