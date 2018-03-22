namespace CPS1.Model
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    [Serializable]
    public static class FunctionComposer
    {
        public static Func<FunctionData, double, double> ComposeFunction(Func<FunctionData, double, double> first, Func<FunctionData, double, double> second, FunctionData secondData, Operation operation)
        {
            var composition = first;
                switch (operation)
                {
                    case Operation.Add:
                        composition = (data, x) => first(data, x) + second(secondData, x);
                        break;
                    case Operation.Subtract:
                        composition = (data, x) => first(data, x) - second(secondData, x);
                        break;
                    case Operation.Multiply:
                        composition = (data, x) => first(data, x) * second(secondData, x);
                        break;
                    case Operation.Divide:
                        composition = (data, x) => first(data, x) / second(secondData, x);
                        break;
            }

            return composition;
        }
    }
}