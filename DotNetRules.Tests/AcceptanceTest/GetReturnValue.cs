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
            static LegacyItem _legacyItem;
            static TargetDomainObject _targetDomainObject;

            Establish context = () =>
            {
                _legacyItem = new LegacyItem { Number = "300" };
                _targetDomainObject = new TargetDomainObject { StringArray = new string[0], Integer = 0 };
            };

            Because of = () => _result = _legacyItem.ApplyPoliciesFor<int, LegacyItem, TargetDomainObject>(_targetDomainObject, policies: new[] { typeof(PolicyWithReturnValue) });

            It return_value_should_be_300 = () => ShouldExtensionMethods.ShouldEqual(_result, 300);
        }
    }
}
