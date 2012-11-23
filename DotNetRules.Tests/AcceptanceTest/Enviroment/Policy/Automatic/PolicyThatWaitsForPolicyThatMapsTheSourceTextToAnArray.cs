using System;
using DotNetRules.Runtime;

namespace DotNetRules.Tests.AcceptanceTest.Enviroment.Policy.Automatic
{
    [Policy(typeof(ExampleTargetObject), typeof(ExampleSourceObject), WaitFor = typeof(PolicyThatMapsTheSourceTextToAnArray))]
    class PolicyThatWaitsForPolicyThatMapsTheSourceTextToAnArray : RelationPolicyBase<ExampleSourceObject, ExampleTargetObject>
    {
        Given isTrue = () => !Source.Number.Equals(Target.Integer.ToString());

        Then replaceTheString = () =>
                                    {
                                        Target.Integer = Convert.ToInt32((string) Source.Number);
                                    };
    }
}
