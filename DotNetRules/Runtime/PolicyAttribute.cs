using System;
using System.Collections.Generic;

namespace DotNetRules.Runtime
{
    [AttributeUsage(AttributeTargets.Class, 
        Inherited = false, 
        AllowMultiple = false)]
    public sealed class PolicyAttribute : Attribute
    {
        public List<Type> Types { get; set; }

        public Type WaitFor { get; set; }

        public bool AutoExecute { get; set; }

        public PolicyAttribute() : this (new Type[0])
        {
        }

        public PolicyAttribute(params Type[] types)
        {
            Types = new List<Type>(types);
            AutoExecute = true;
        }
    }
}
