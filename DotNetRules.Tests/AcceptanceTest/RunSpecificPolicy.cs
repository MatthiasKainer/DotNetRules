using System;
using System.Collections.Generic;
using System.Linq;
using DotNetRules.Tests.AcceptanceTest.Enviroment.Policy.Automatic;
using Machine.Specifications;
using DotNetRules.Tests.AcceptanceTest.Enviroment;
using DotNetRules.Tests.AcceptanceTest.Enviroment.Policy;

namespace DotNetRules.Tests.AcceptanceTest
{
    [Subject("Executor")]
    class RunSpecificPolicy
    {
        class When_only_one_value_is_different
        {
            static ExecutionTrace _result;
            static ExampleSourceObject _exampleSourceObject;
            static ExampleTargetObject _exampleTargetObject;

            Establish context = () =>
            {
                _exampleSourceObject = new ExampleSourceObject();
                _exampleTargetObject = new ExampleTargetObject { StringArray = new string[0], Integer = 0 };
            };

            Because of = () => _result = Executor.Apply(_exampleSourceObject, _exampleTargetObject, policies: new[] { typeof(PolicyThatMapsTheSourceTextToAnArray) });

            It should_have_executed_one_policy = () => _result.Called.ShouldEqual(1);

            It should_have_executed_the_ExamplePolicy = () => _result.By.All(_ => _ == typeof(PolicyThatMapsTheSourceTextToAnArray)).ShouldBeTrue();
        }
    }
}
