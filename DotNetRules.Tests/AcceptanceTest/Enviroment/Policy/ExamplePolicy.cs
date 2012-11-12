using System;
using DotNetRules.Runtime;

namespace DotNetRules.Tests.AcceptanceTest.Enviroment.Policy
{
    [Policy(typeof(TargetDomainObject), typeof(LegacyItem))]
    class ExamplePolicy : RelationPolicyBase<LegacyItem, TargetDomainObject>
    {
        Given isTrue = () => !Source.Text.Trim().Equals(string.Join(" ", Target.StringArray));

        Then replaceTheString = () =>
        {
            Target.StringArray = Source.Text.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        };
    }
}
