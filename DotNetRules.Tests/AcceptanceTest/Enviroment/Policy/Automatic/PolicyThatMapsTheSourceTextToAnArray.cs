using System;
using DotNetRules.Runtime;

namespace DotNetRules.Tests.AcceptanceTest.Enviroment.Policy.Automatic
{
    [Policy(typeof(ExampleTargetObject), typeof(ExampleSourceObject))]
    class PolicyThatMapsTheSourceTextToAnArray : RelationPolicyBase<ExampleSourceObject, ExampleTargetObject>
    {
        Given isTrue = () => !Source.Text.Trim().Equals(string.Join(" ", Target.StringArray));

        Then replaceTheString = () =>
        {
            Target.StringArray = Source.Text.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        };
    }
}
