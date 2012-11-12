using System;

namespace DotNetRules.Runtime
{
    public class PolicyBase<TSubject>
    {
        public PolicyBase()
        {
            var constructorInfo = typeof(TSubject).GetConstructor(Type.EmptyTypes);
            if (constructorInfo != null)
            {
                Subject = (TSubject)constructorInfo.Invoke(null);
            }
        }

        public static TSubject Subject { get; set; }
    }
}
