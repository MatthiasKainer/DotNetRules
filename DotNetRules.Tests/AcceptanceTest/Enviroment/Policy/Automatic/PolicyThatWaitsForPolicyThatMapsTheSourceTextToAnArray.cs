using System;
using DotNetRules.Runtime;

namespace DotNetRules.Tests.AcceptanceTest.Enviroment.Policy.Automatic
{
    [Policy(typeof(TargetDomainObject), typeof(LegacyItem), WaitFor = typeof(PolicyThatMapsTheSourceTextToAnArray))]
    class PolicyThatWaitsForPolicyThatMapsTheSourceTextToAnArray : RelationPolicyBase<LegacyItem, TargetDomainObject>
    {
        Given isTrue = () => !Source.Number.Equals(Target.Integer.ToString());

        Then replaceTheString = () =>
                                    {
                                        Target.Integer = Convert.ToInt32((string) Source.Number);
                                    };
    }
}
