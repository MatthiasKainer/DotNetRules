using DotNetRules.Runtime;

namespace DotNetRules.Tests.AcceptanceTest.Enviroment.Policy.Manual
{
    [Policy(typeof(TargetDomainObject), typeof(LegacyItem), AutoExecute = false)]
    class PolicyWithOrThatSetsSourceNumberToOne : RelationPolicyBase<LegacyItem, TargetDomainObject>
    {
        Given numbersAreEqual = () => Source.Number.Equals(Target.Integer.ToString());

        Or stringIsNullOrEmpty = () => string.IsNullOrEmpty(Source.Text);

        Then replaceTheString = () =>
                                    {
                                        Source.Number = "1";
                                    };
    }
}
