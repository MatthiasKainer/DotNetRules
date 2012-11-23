using System;
using DotNetRules.Runtime;

namespace DotNetRules.Tests.AcceptanceTest.Enviroment.Policy.Manual
{
    [Policy(typeof(TargetDomainObject), typeof(LegacyItem), AutoExecute= false)]
    class PolicyWithReturnValueThatMapsSourceNumberToTargetInteger : RelationPolicyBase<LegacyItem, TargetDomainObject>
    {
        Given isTrue = () => !Source.Number.Equals(Target.Integer.ToString());

        Then replaceTheString = () =>
        {
            Target.Integer = Convert.ToInt32(Source.Number);
        };

        Return<int> @return = () => Target.Integer;
    }
}
