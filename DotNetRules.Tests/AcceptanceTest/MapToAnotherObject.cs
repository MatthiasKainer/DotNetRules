using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DotNetRules.Tests.AcceptanceTest.Enviroment;
using DotNetRules.Tests.AcceptanceTest.Enviroment.Policy.Automatic;
using DotNetRules.Web.Mvc;
using Machine.Specifications;

namespace DotNetRules.Tests.AcceptanceTest
{
    [Subject("Executor")]
    class MapToAnotherObject
    {
        class When_two_objects_are_same_and_values_are_different
        {
            static ExecutionTrace _result;
            static ExampleSourceObject _exampleSourceObject;
            static ExampleSourceObject _exampleTargetObject;

            Establish context = () =>
            {
                _exampleSourceObject = new ExampleSourceObject { Text = "text", Number = "100" };
                _exampleTargetObject = new ExampleSourceObject { Text = "", Number = "0" };
            };

            Because of = () =>
            {
                _result = _exampleSourceObject.ApplyPoliciesFor(_exampleTargetObject, policies: new[] { typeof(PolicyThatMapsTheSameObject) });
            };

            It should_have_executed_one_policy = () => _result.Called.ShouldEqual(1);

            It should_have_executed_the_PolicyThatMapsTheSameObject = () => _result.By.Any(_ => _ == typeof(PolicyThatMapsTheSameObject)).ShouldBeTrue();

            It should_have_changed_the_source_text_to_the_array = () => _exampleTargetObject.Text.ShouldEqual("text");

            It should_not_have_changed_the_source_number_to_the_int = () => _exampleTargetObject.Number.ShouldEqual("0");
        }

        class When_two_values_are_different
        {
            static ExecutionTrace _result;
            static ExampleSourceObject _exampleSourceObject;
            static ExampleTargetObject _exampleTargetObject;

            Establish context = () =>
            {
                _exampleSourceObject = new ExampleSourceObject { Text = "text", Number = "100" };
                _exampleTargetObject = new ExampleTargetObject { StringArray = new string[0], Integer = 0 };
            };

            Because of = () =>
            {
                _result = Executor.Apply(_exampleSourceObject, _exampleTargetObject);
            };

            It should_have_executed_two_policies = () => _result.Called.ShouldEqual(2);

            It should_have_executed_the_ADependendPolicy = () => _result.By.Any(_ => _ == typeof(PolicyThatWaitsForPolicyThatMapsTheSourceTextToAnArray)).ShouldBeTrue();

            It should_have_executed_the_ExamplePolicy = () => _result.By.Any(_ => _ == typeof(PolicyThatMapsTheSourceTextToAnArray)).ShouldBeTrue();

            It should_have_executed_the_ExamplePolicy_first =
                () => _result.By.Peek().ShouldEqual(typeof(PolicyThatMapsTheSourceTextToAnArray));

            It should_have_changed_the_source_text_to_the_array = () => _exampleTargetObject.StringArray.ShouldEqual(new[] { "text" });

            It should_have_changed_the_source_number_to_the_int = () => _exampleTargetObject.Integer.ShouldEqual(100);
        }

        class When_two_values_are_different_and_mvc_modelstate_is_used
        {
            static ExecutionTrace _result;
            static ExampleSourceObject _exampleSourceObject;
            static ExampleTargetObject _exampleTargetObject;

            Establish context = () =>
            {
                _dictionary = new ModelStateDictionary();
                _exampleSourceObject = new ExampleSourceObject { Text = "text", Number = "100" };
                _exampleTargetObject = new ExampleTargetObject { StringArray = new string[0], Integer = 0 };
            };

            Because of = () =>
            {
                _result = _dictionary.ApplyPoliciesFor(_exampleSourceObject, _exampleTargetObject);
            };

            It should_have_executed_two_policies = () => _result.Called.ShouldEqual(2);

            It should_have_executed_the_ADependendPolicy = () => _result.By.Any(_ => _ == typeof(PolicyThatWaitsForPolicyThatMapsTheSourceTextToAnArray)).ShouldBeTrue();

            It should_have_executed_the_ExamplePolicy = () => _result.By.Any(_ => _ == typeof(PolicyThatMapsTheSourceTextToAnArray)).ShouldBeTrue();

            It should_have_executed_the_ExamplePolicy_first =
                () => _result.By.Peek().ShouldEqual(typeof(PolicyThatMapsTheSourceTextToAnArray));

            It should_have_changed_the_source_text_to_the_array = () => _exampleTargetObject.StringArray.ShouldEqual(new[] { "text" });

            It should_have_changed_the_source_number_to_the_int = () => _exampleTargetObject.Integer.ShouldEqual(100);
            static ModelStateDictionary _dictionary;
        }
    }
}
