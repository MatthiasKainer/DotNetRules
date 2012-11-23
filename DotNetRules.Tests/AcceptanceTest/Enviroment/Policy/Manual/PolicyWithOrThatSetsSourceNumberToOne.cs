using DotNetRules.Runtime;

namespace DotNetRules.Tests.AcceptanceTest.Enviroment.Policy.Manual
{
    [Policy(typeof(ExampleTargetObject), typeof(ExampleSourceObject), AutoExecute = false)]
    class PolicyWithOrThatSetsSourceNumberToOne : RelationPolicyBase<ExampleSourceObject, ExampleTargetObject>
    {
        Given numbersAreEqual = () => Source.Number.Equals(Target.Integer.ToString());

        Or stringIsNullOrEmpty = () => string.IsNullOrEmpty(Source.Text);

        Then replaceTheString = () =>
                                    {
                                        Source.Number = "1";
                                    };
    }
}
