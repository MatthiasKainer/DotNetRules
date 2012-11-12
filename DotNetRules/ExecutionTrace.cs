using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetRules
{
    public class ExecutionTrace : IExecutionTrace
    {
        internal void Initalize<T>(ExecutionTrace<T> executionTrace)
        {
            Called = executionTrace.Called;
            By = executionTrace.By;
        }

        public int Called { get; set; }
        public Queue<Type> By { get; set; }
        public bool WasConditionMetFor(Type policy)
        {
            return By.Any(_ => _ == policy);
        }
    }

    public class ExecutionTrace<T> : IExecutionTrace
    {
        public int Called { get; set; }
        public Queue<Type> By { get; set; }
        public T ReturnType { get; set; }

        public bool WasConditionMetFor(Type policy)
        {
            return By.Any(_ => _ == policy);
        }

        public static implicit operator T(ExecutionTrace<T> trace)
        {
            return trace.ReturnType;
        }

        public static explicit operator ExecutionTrace(ExecutionTrace<T> trace)
        {
            var executionTrace = new ExecutionTrace();
            executionTrace.Initalize(trace);
            return executionTrace;
        }

        public ExecutionTrace()
        {
            By = new Queue<Type>();
        }
    }
}