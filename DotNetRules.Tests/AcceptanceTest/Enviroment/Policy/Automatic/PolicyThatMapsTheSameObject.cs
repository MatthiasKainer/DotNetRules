using System;
using DotNetRules.Runtime;

namespace DotNetRules.Tests.AcceptanceTest.Enviroment.Policy.Automatic
{
    [Policy(typeof(ExampleSourceObject), typeof(ExampleSourceObject))]
    class PolicyThatMapsTheSameObject : RelationPolicyBase<ExampleSourceObject, ExampleSourceObject>
    {
        Given isTrue = () => Source.Text != Target.Text;

        Then replaceTheString = () =>
        {
            Target.Text = Source.Text;
        };
    }
}
