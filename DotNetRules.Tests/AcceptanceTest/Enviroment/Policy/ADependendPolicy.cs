using System;
using DotNetRules.Runtime;

namespace DotNetRules.Tests.AcceptanceTest.Enviroment.Policy
{
    [Policy(typeof(TargetDomainObject), typeof(LegacyItem), WaitFor = typeof(ExamplePolicy))]
    class ADependendPolicy : RelationPolicyBase<LegacyItem, TargetDomainObject>
    {
        Given isTrue = () => !Source.Number.Equals(Target.Integer.ToString());

        Then replaceTheString = () =>
                                    {
                                        Target.Integer = Convert.ToInt32((string) Source.Number);
                                    };
    }
}
