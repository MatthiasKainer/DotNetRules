using DotNetRules.Runtime;

namespace DotNetRules.Tests.AcceptanceTest.Enviroment.Policy.Manual
{
    [Policy(typeof(ExampleSourceObject), AutoExecute = false)]
    class PolicyThatsAlwaysReturningTrue : PolicyBase<ExampleSourceObject>
    {
        Given that = () => true;

        Then doNothing = () => { };
    }
}
