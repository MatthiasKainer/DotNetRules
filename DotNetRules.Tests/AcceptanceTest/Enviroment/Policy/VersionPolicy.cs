using System;
using DotNetRules.Runtime;

namespace DotNetRules.Tests.AcceptanceTest.Enviroment.Policy
{
    [Policy(typeof(TargetDomainObject), typeof(LegacyItem))]
    internal class VersionPolicy : RelationPolicyBase<LegacyItem, TargetDomainObject>
    {
        Given versionsAreNotTheSame = () =>
                                      Convert.ToInt32((string) Source.Version) != Target.Version;

        Then updateTheVersion = () => Target.Version = Convert.ToInt32((string) Source.Version);

        Return<LegacyItem> @return = () => Source;
    }
}