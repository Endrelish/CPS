namespace CPS1.Model.SignalData
{
    public struct Required
    {
        public Required(
            bool startTime,
            bool amplitude,
            bool period,
            bool duration,
            bool dutyCycle,
            bool samples,
            bool continuous,
            bool probability)
        {
            StartTime = startTime;
            Amplitude = amplitude;
            Period = period;
            Duration = duration;
            DutyCycle = dutyCycle;
            Samples = samples;
            Continuous = continuous;
            Probability = probability;
        }

        public bool StartTime { get; }

        public bool Amplitude { get; }

        public bool Period { get; }

        public bool Duration { get; }

        public bool DutyCycle { get; }

        public bool Samples { get; }

        public bool Continuous { get; }

        public bool Probability { get; }
    }
}