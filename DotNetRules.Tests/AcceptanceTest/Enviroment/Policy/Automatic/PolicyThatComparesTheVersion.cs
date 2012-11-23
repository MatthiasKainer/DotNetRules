using System;
using DotNetRules.Runtime;

namespace DotNetRules.Tests.AcceptanceTest.Enviroment.Policy.Automatic
{
    [Policy(typeof(ExampleTargetObject), typeof(ExampleSourceObject))]
    internal class PolicyThatComparesTheVersion : RelationPolicyBase<ExampleSourceObject, ExampleTargetObject>
    {
        Given versionsAreNotTheSame = () =>
                                      Convert.ToInt32((string) Source.Version) != Target.Version;

        Then updateTheVersion = () => Target.Version = Convert.ToInt32((string) Source.Version);

        Return<ExampleSourceObject> @return = () => Source;
    }
}