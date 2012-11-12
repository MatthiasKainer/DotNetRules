using System;

namespace DotNetRules.Runtime
{
    public class RelationPolicyBase<TSource, TTarget>
    {
        public RelationPolicyBase()
        {
            Source = InvokeConstructor<TSource>();
            Target = InvokeConstructor<TTarget>();
        }

        public static TSource Source { get; set; }

        public static TTarget Target { get; set; }
        
        private TO InvokeConstructor<TO>()
        {
            var constructorInfo = typeof(TO).GetConstructor(Type.EmptyTypes);
            return constructorInfo != null ? (TO) constructorInfo.Invoke(null) : default(TO);
        }
    }
}
