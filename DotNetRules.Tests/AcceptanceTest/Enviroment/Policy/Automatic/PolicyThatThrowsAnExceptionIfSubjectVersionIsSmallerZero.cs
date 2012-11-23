using DotNetRules.Runtime;

namespace DotNetRules.Tests.AcceptanceTest.Enviroment.Policy.Automatic
{
    [Policy(typeof(TargetDomainObject))]
    class PolicyThatThrowsAnExceptionIfSubjectVersionIsSmallerZero : PolicyBase<TargetDomainObject>
    {
        Given versionIsLessThenZero = () => Subject.Version < 0;

        Then throwAnInvalidStateException = () =>
        {
            throw new InvalidStateException();
        };
    }
}
