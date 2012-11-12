using System;

namespace DotNetRules.Runtime
{
    public abstract class BasePolicy
    {
        protected TTarget InvokeConstructor<TTarget>()
        {
            var constructorInfo = typeof(TTarget).GetConstructor(Type.EmptyTypes);
            if (constructorInfo == null)
            {
                throw new Exception(string.Format("Parameterless required for type {0}", typeof(TTarget)));
            }

            return (TTarget)constructorInfo.Invoke(null);
        }
    }
}