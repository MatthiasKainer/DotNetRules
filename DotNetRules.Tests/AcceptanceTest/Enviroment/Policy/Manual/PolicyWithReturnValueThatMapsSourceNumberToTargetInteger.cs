using System;
using DotNetRules.Runtime;

namespace DotNetRules.Tests.AcceptanceTest.Enviroment.Policy.Manual
{
    [Policy(typeof(ExampleTargetObject), typeof(ExampleSourceObject), AutoExecute= false)]
    class PolicyWithReturnValueThatMapsSourceNumberToTargetInteger : RelationPolicyBase<ExampleSourceObject, ExampleTargetObject>
    {
        Given isTrue = () => !Source.Number.Equals(Target.Integer.ToString());

        Then replaceTheString = () =>
        {
            Target.Integer = Convert.ToInt32(Source.Number);
        };

        Return<int> @return = () => Target.Integer;
    }
}
