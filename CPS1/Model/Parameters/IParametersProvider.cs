using System.Collections.Generic;

namespace CPS1.Model.Parameters
{
    public interface IParametersProvider
    {
        IEnumerable<Parameter> Parameters { get; }
    }
}