using System;
using System.Linq;
using Machine.Specifications;
using DotNetRules.Tests.AcceptanceTest.Enviroment;
using DotNetRules.Tests.AcceptanceTest.Enviroment.Policy;

namespace DotNetRules.Tests.AcceptanceTest
{
    // ReSharper disable InconsistentNaming
    // ReSharper disable UnusedMember.Local
    [Subject("Executor")]
    class RunAllPolicies
    {
        class When_only_one_value_is_different
        {
            static ExecutionTrace _result;
            static LegacyItem _legacyItem;
            static TargetDomainObject _targetDomainObject;

            Establish context = () =>
                {
                    _legacyItem = new LegacyItem();
                    _targetDomainObject = new TargetDomainObject
                                            {StringArray = new string[0], Integer = 0};
                };

            Because of = () => _result = Executor.Apply(_legacyItem, _targetDomainObject);

            It should_have_executed_one_policy = () => _result.Called.ShouldEqual(1);

            It should_have_executed_the_ExamplePolicy = () => _result.By.All(_ => _ == typeof(ExamplePolicy)).ShouldBeTrue();
        }

        class When_two_values_are_different
        {
            static ExecutionTrace _result;
            static LegacyItem _legacyItem;
            static TargetDomainObject _targetDomainObject;

            Establish context = () =>
            {
                _legacyItem = new LegacyItem { Text = "text", Number = "100" };
                _targetDomainObject = new TargetDomainObject { StringArray = new string[0], Integer = 0 };
            };

            Because of = () =>
                             {
                                 _result = Executor.Apply(_legacyItem, _targetDomainObject);
                             };

            It should_have_executed_two_policies = () => _result.Called.ShouldEqual(2);

            It should_have_executed_the_ADependendPolicy = () => _result.By.Any(_ => _ == typeof(ADependendPolicy)).ShouldBeTrue();

            It should_have_executed_the_ExamplePolicy = () => _result.By.Any(_ => _ == typeof(ExamplePolicy)).ShouldBeTrue();

            It should_have_executed_the_ExamplePolicy_first =
                () => _result.By.Peek().ShouldEqual(typeof(ExamplePolicy));
        }
    }
}
