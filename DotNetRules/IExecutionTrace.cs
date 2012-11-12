using System;
using System.Collections.Generic;

namespace DotNetRules
{
    public interface IExecutionTrace
    {
        int Called { get; set; }
        Queue<Type> By { get; set; }
        bool WasConditionMetFor(Type policy);
    }
}