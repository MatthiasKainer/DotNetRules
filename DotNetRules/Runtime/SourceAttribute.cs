using System;

namespace DotNetRules.Runtime
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class SourceAttribute : Attribute
    {
        // This is a positional argument
        public SourceAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}