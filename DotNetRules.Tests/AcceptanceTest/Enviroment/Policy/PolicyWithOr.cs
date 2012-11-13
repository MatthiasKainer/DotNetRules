using System;
using DotNetRules.Runtime;

namespace DotNetRules.Tests.AcceptanceTest.Enviroment.Policy
{
    [Policy(typeof(TargetDomainObject), typeof(LegacyItem), AutoExecute = false)]
    class PolicyWithOr : RelationPolicyBase<LegacyItem, TargetDomainObject>
    {
        Given isTrue = () => !Source.Number.Equals(Target.Integer.ToString());

        Or isFalse = () => string.IsNullOrEmpty(Source.Text);

        Then replaceTheString = () =>
                                    {
                                        Source.Number = "1";
                                    };
    }
}
