namespace CPS1.Model
{
    using System;

    [Serializable]
    public class OperationData
    {
        public OperationData(
            Func<FunctionData, double, double> firstOperand,
            Func<FunctionData, double, double> secondOperand = null,
            Operation operation = Operation.Add)
        {
            this.FirstOperand = firstOperand;
            this.SecondOperand = secondOperand;
            this.Operation = operation;
        }

        private Func<FunctionData, double, double> FirstOperand { get; }

        public Operation Operation { get; set; }

        public Func<FunctionData, double, double> SecondOperand { get; set; }

        public Func<FunctionData, double, double> Compose(FunctionData firstOperandData, FunctionData secondOperandData)
        {
            Func<FunctionData, double, double> function = (data, a) =>
                {
                    if (this.SecondOperand == null)
                    {
                        return this.FirstOperand(data, a);
                    }

                    switch (this.Operation)
                    {
                        case Operation.Add:
                            return this.FirstOperand(firstOperandData, a) + this.SecondOperand(secondOperandData, a);
                        case Operation.Subtract:
                            return this.FirstOperand(firstOperandData, a) - this.SecondOperand(secondOperandData, a);
                        case Operation.Multiply:
                            return this.FirstOperand(firstOperandData, a) * this.SecondOperand(secondOperandData, a);
                        case Operation.Divide:
                            return this.FirstOperand(firstOperandData, a) / this.SecondOperand(secondOperandData, a);
                        default:
                            return 0;
                    }
                };
            return function;
        }
    }
}