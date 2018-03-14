namespace CPS1.Functions
{
    public struct Required
    {
        public Required(bool startTime, bool amplitude, bool period, bool duration, bool dutyCycle, bool samples)
        {
            this.StartTime = startTime;
            this.Amplitude = amplitude;
            this.Period = period;
            this.Duration = duration;
            this.DutyCycle = dutyCycle;
            this.Samples = samples;
        }

        public bool StartTime { get; }

        public bool Amplitude { get; }

        public bool Period { get; }

        public bool Duration { get; }

        public bool DutyCycle { get; }

        public bool Samples { get; }
    }
}