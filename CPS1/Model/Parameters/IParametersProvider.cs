namespace CPS1.Model.Parameters
{
    using System.Collections;
    using System.Collections.Generic;

    public interface IParametersProvider
    {
        IEnumerable<Parameter> Parameters { get; }
    }
}