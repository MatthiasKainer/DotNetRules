using DotNetRules.Tests.AcceptanceTest.Enviroment.Policy.Manual;
using Machine.Specifications;
using DotNetRules.Tests.AcceptanceTest.Enviroment;
using DotNetRules.Tests.AcceptanceTest.Enviroment.Policy;

namespace DotNetRules.Tests.AcceptanceTest
{
    [Subject("Executor")]
    class GetReturnValue
    {
        class When_Policy_Is_Specified_Explicitly_As_Policy_With_Return_Value
        {
            static ExecutionTrace<int> _result;
            static ExampleSourceObject _exampleSourceObject;
            static ExampleTargetObject _exampleTargetObject;

            Establish context = () =>
            {
                _exampleSourceObject = new ExampleSourceObject { Number = "300" };
                _exampleTargetObject = new ExampleTargetObject { StringArray = new string[0], Integer = 0 };
            };

            Because of = () => _result = _exampleSourceObject.ApplyPoliciesFor<int, ExampleSourceObject, ExampleTargetObject>(_exampleTargetObject, policies: new[] { typeof(PolicyWithReturnValueThatMapsSourceNumberToTargetInteger) });

            It return_value_should_be_300 = () => ShouldExtensionMethods.ShouldEqual(_result, 300);
        }
    }
}
