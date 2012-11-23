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
            static TargetDomainObject _targetDomainObject;
            static Exception exception;

            Establish context = () =>
                                    {
                                        _targetDomainObject = new TargetDomainObject();
                                    };

            Because of = () => exception = Catch.Exception(() => _result = Executor.Apply(_targetDomainObject));

            It should_fullfill_the_condition = () => _result.WasConditionMetFor(typeof(PolicyThatThrowsAnExceptionIfSubjectVersionIsSmallerZero));

            It should_not_throw_an_exception = () => exception.ShouldBeNull();
        }

        class When_TargetDomainObject_is_invalid
        {
            static ExecutionTrace _result;
            static TargetDomainObject _targetDomainObject;
            static Exception exception;

            Establish context = () =>
            {
                _targetDomainObject = new TargetDomainObject() { Version = -1 };
            };

            Because of = () => exception = Catch.Exception(() => _result = Executor.Apply(_targetDomainObject));

            It should_throw_an_exception = () => exception.ShouldNotBeNull();
        }

        class When_the_values_are_different
        {
            static TestContext _testContext;
            static LegacyItem _legacyItem;
            static TargetDomainObject _targetDomainObject;

            Establish context = () =>
                                    {
                                        _testContext = new TestContext(typeof(PolicyThatMapsTheSourceTextToAnArray));
                                        _legacyItem = new LegacyItem {Text = "text"};
                                        _targetDomainObject = new TargetDomainObject
                                                                {StringArray = new string[0]};
                                    };

            Because of = () => _testContext.Execute(_legacyItem, _targetDomainObject);

            It should_fullfill_the_condition = () => _testContext.WasConditionFullfilled().ShouldBeTrue();

            It should_have_the_new_value = () => _targetDomainObject.StringArray.ShouldEqual(new[] {"text"});
        }

        class When_the_values_are_the_same
        {
            static TestContext _testContext;
            static LegacyItem _legacyItem;
            static TargetDomainObject _targetDomainObject;

            Establish context = () =>
            {
                _testContext = new TestContext(typeof(PolicyThatComparesTheVersion));
                _legacyItem = new LegacyItem {Version = "1"};
                _targetDomainObject = new TargetDomainObject { Version = 1 };
            };

            Because of = () => _testContext.Execute(_legacyItem, _targetDomainObject);

            It should_not_fullfill_the_condition = () => 
                _testContext.WasConditionFullfilled().ShouldBeFalse();
        }
    }
}
