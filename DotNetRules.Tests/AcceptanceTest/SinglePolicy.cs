using System;
using DotNetRules.Tests.AcceptanceTest.Enviroment.Policy.Automatic;
using Machine.Specifications;
using DotNetRules.TestFramework;
using DotNetRules.Tests.AcceptanceTest.Enviroment;
using DotNetRules.Tests.AcceptanceTest.Enviroment.Policy;

namespace DotNetRules.Tests.AcceptanceTest
{
    // ReSharper disable InconsistentNaming
    // ReSharper disable UnusedMember.Local
    [Subject("PolicyThatMapsTheSourceTextToAnArray")]
    class SinglePolicy 
    {
        class When_TargetDomainObject_is_valid
        {
            static ExecutionTrace _result = new ExecutionTrace();
            static ExampleTargetObject _exampleTargetObject;
            static Exception exception;

            Establish context = () =>
                                    {
                                        _exampleTargetObject = new ExampleTargetObject();
                                    };

            Because of = () => exception = Catch.Exception(() => _result = Executor.Apply(_exampleTargetObject));

            It should_fullfill_the_condition = () => _result.WasConditionMetFor(typeof(PolicyThatThrowsAnExceptionIfSubjectVersionIsSmallerZero));

            It should_not_throw_an_exception = () => exception.ShouldBeNull();
        }

        class When_TargetDomainObject_is_invalid
        {
            static ExecutionTrace _result;
            static ExampleTargetObject _exampleTargetObject;
            static Exception exception;

            Establish context = () =>
            {
                _exampleTargetObject = new ExampleTargetObject() { Version = -1 };
            };

            Because of = () => exception = Catch.Exception(() => _result = Executor.Apply(_exampleTargetObject));

            It should_throw_an_exception = () => exception.ShouldNotBeNull();
        }

        class When_the_values_are_different
        {
            static TestContext _testContext;
            static ExampleSourceObject _exampleSourceObject;
            static ExampleTargetObject _exampleTargetObject;

            Establish context = () =>
                                    {
                                        _testContext = new TestContext(typeof(PolicyThatMapsTheSourceTextToAnArray));
                                        _exampleSourceObject = new ExampleSourceObject {Text = "text"};
                                        _exampleTargetObject = new ExampleTargetObject
                                                                {StringArray = new string[0]};
                                    };

            Because of = () => _testContext.Execute(_exampleSourceObject, _exampleTargetObject);

            It should_fullfill_the_condition = () => _testContext.WasConditionFullfilled().ShouldBeTrue();

            It should_have_the_new_value = () => _exampleTargetObject.StringArray.ShouldEqual(new[] {"text"});
        }

        class When_the_values_are_the_same
        {
            static TestContext _testContext;
            static ExampleSourceObject _exampleSourceObject;
            static ExampleTargetObject _exampleTargetObject;

            Establish context = () =>
            {
                _testContext = new TestContext(typeof(PolicyThatComparesTheVersion));
                _exampleSourceObject = new ExampleSourceObject {Version = "1"};
                _exampleTargetObject = new ExampleTargetObject { Version = 1 };
            };

            Because of = () => _testContext.Execute(_exampleSourceObject, _exampleTargetObject);

            It should_not_fullfill_the_condition = () => 
                _testContext.WasConditionFullfilled().ShouldBeFalse();
        }
    }
}
