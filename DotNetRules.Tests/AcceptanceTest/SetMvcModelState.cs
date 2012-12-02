using System.Linq;
using System.Web.Mvc;
using DotNetRules.Web.Mvc;
using DotNetRules.Tests.AcceptanceTest.Enviroment;
using DotNetRules.Tests.AcceptanceTest.Enviroment.Policy.Manual;
using Machine.Specifications;

namespace DotNetRules.Tests.AcceptanceTest
{
    class SetMvcModelState
    {
        class When_the_model_is_valid
        {
            static ExecutionTrace _result;
            static ExampleSourceObject _exampleSourceObject;
            static ModelStateDictionary _dictionary;

            Establish context = () =>
            {
                _dictionary = new ModelStateDictionary();
                Executor.Settings.CatchExceptions = true;
                _exampleSourceObject = new ExampleSourceObject();
            };

            Because of = () => _result = _dictionary.ApplyPolicies(_exampleSourceObject, policies: new[] { typeof(PolicyThatsAlwaysReturningTrue) });

            It should_have_executed_one_policy = () => _result.Called.ShouldEqual(1);

            It should_have_executed_the_ExamplePolicy = () => _result.By.All(_ => _ == typeof(PolicyThatsAlwaysReturningTrue)).ShouldBeTrue();

            It should_not_have_thrown_an_exception = () => Executor.Exceptions.Any().ShouldBeFalse();

            It should_have_a_valid_model_state = () => _dictionary.IsValid.ShouldBeTrue();
        }

        class When_the_model_is_invalid 
        {
            static ExecutionTrace _result;
            static ExampleSourceObject _exampleSourceObject;
            static ModelStateDictionary _dictionary;

            Establish context = () =>
            {
                _dictionary = new ModelStateDictionary();
                Executor.Settings.CatchExceptions = true;
                _exampleSourceObject = new ExampleSourceObject();
            };

            Because of = () => _result = _dictionary.ApplyPolicies(_exampleSourceObject, policies: new[] { typeof(PolicyForMvcThatThrowsAnException) });

            It should_have_executed_one_policy = () => _result.Called.ShouldEqual(1);

            It should_have_executed_the_ExamplePolicy = () => _result.By.All(_ => _ == typeof(PolicyForMvcThatThrowsAnException)).ShouldBeTrue();

            It should_have_thrown_an_exception = () => Executor.Exceptions.Any().ShouldBeTrue();

            It should_have_an_invalid_model_state = () => _dictionary.IsValid.ShouldBeFalse();

            It should_have_a_invalid_model_state_for_a_specific_property = () =>
            {
                _dictionary.Keys.FirstOrDefault(_ => _ == "ModelProperty").ShouldNotBeNull();
                _dictionary["ModelProperty"].Errors.ShouldNotBeEmpty();
            };
        }
    }
}
