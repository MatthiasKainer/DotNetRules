using System;
using DotNetRules.Runtime;

namespace DotNetRules.Tests.AcceptanceTest.Enviroment.Policy.Automatic
{
    [Policy(typeof(TargetDomainObject), typeof(LegacyItem))]
    class PolicyThatMapsTheSourceTextToAnArray : RelationPolicyBase<LegacyItem, TargetDomainObject>
    {
        Given isTrue = () => !Source.Text.Trim().Equals(string.Join(" ", Target.StringArray));

        Then replaceTheString = () =>
        {
            Target.StringArray = Source.Text.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        };
    }
}
