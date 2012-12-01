using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotNetRules
{
    public interface IExecutionTrace
    {
        int Called { get; set; }
        Queue<Type> By { get; set; }
        Assembly CurrentAssembly { get; }
        bool WasConditionMetFor(Type policy);
    }
}