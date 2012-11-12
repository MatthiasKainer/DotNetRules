using DotNetRules.Runtime;

namespace DotNetRules.Tests.AcceptanceTest.Enviroment.Policy
{
    [Policy(typeof(TargetDomainObject))]
    class APolicyForASingleObject : PolicyBase<TargetDomainObject>
    {
        Given versionIsLessThenZero = () => Subject.Version < 0;

        Then throwAnInvalidStateException = () =>
        {
            throw new InvalidStateException();
        };
    }
}
