namespace CPS1.Model
{
    using System;

    public class OperationData
    {
        public OperationData FirstData { get; set; }
        public OperationData SecondData { get; set; }

        public Operation operation;
        public Signal FirstType { get; set; }
        public Signal SecondType { get; set; }
        public double FirstAmplitude { get; set; }
        public double SecondAmplitude { get; set; }
        public double FirstStartTime { get; set; }
        public double SecondStartTime { get; set; }
        public double FirstPeriod { get; set; }
        public double SecondPeriod { get; set; }
        public double FirstDutyCycle { get; set; }
        public double SecondDutyCycle { get; set; }
        public double FirstProbability { get; set; }
        public double SecondProbability { get; set; }
        public double FirstDuration { get; set; }
        public double SecondDuration { get; set; }

        private FunctionData firstFunctionData;
        private FunctionData secondFunctionData;



        public double Compose(double t)
        {
            double firstValue = 0;
            if (FirstType == Signal.Composite) firstValue = FirstData.Compose(t);
            else
            {
                firstValue = AvailableFunctions.GetFunction(FirstFunctionData.Type)(FirstFunctionData, t);
            }

            double secondValue = 0;
            if (SecondType == Signal.Composite) secondValue = SecondData.Compose(t);
            else
            {
                secondValue = AvailableFunctions.GetFunction(SecondFunctionData.Type)(SecondFunctionData, t);
            }

            switch (operation)
            {
                case Operation.Add:
                    return firstValue + secondValue;
                case Operation.Subtract:
                    return firstValue - secondValue;
                case Operation.Multiply:
                    return firstValue * secondValue;
                case Operation.Divide:
                    if (Math.Abs(secondValue) < 10E-10) return double.MaxValue;
                    else return firstValue / secondValue;
                default:
                    return 0;
            }
        }

        public int FirstHistogramIntervals { get; set; }

        public int FirstSamples { get; set; }

        public FunctionData FirstFunctionData
        {
            get
            {
                if (this.firstFunctionData == null && FirstType != Signal.Composite)
                {
                    this.firstFunctionData = new FunctionData(
                        FirstStartTime,
                        FirstAmplitude,
                        FirstPeriod,
                        FirstDuration,
                        FirstDutyCycle,
                        FirstSamples,
                        FirstHistogramIntervals,
                        FirstProbability);
                        this.firstFunctionData.Type = FirstType;
                    //Generator.GenerateSignal(this.firstFunctionData);
                }
                return this.firstFunctionData;
            }
        }

        public FunctionData SecondFunctionData
        {
            get
            {
                if (this.firstFunctionData == null && SecondType != Signal.Composite) 
                {
                    this.secondFunctionData = new FunctionData(
                        SecondStartTime,
                        SecondAmplitude,
                        SecondPeriod,
                        SecondDuration,
                        SecondDutyCycle,
                        SecondSamples,
                        SecondHistogramIntervals,
                        SecondProbability);
                    this.secondFunctionData.Type = SecondType;
                    //Generator.GenerateSignal(this.secondFunctionData);
                }
                return this.secondFunctionData;
            }
        }

        public int SecondSamples { get; set; }

        public int SecondHistogramIntervals { get; set; }
    }
}