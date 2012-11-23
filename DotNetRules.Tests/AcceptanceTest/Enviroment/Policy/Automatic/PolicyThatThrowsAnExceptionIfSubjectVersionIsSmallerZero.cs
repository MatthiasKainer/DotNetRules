using DotNetRules.Runtime;

namespace DotNetRules.Tests.AcceptanceTest.Enviroment.Policy.Automatic
{
    [Policy(typeof(ExampleTargetObject))]
    class PolicyThatThrowsAnExceptionIfSubjectVersionIsSmallerZero : PolicyBase<ExampleTargetObject>
    {
        Given versionIsLessThenZero = () => Subject.Version < 0;

        Then throwAnInvalidStateException = () =>
        {
            throw new InvalidStateException();
        };
    }
}
