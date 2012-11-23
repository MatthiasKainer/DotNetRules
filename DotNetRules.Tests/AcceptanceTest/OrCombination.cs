using DotNetRules.Tests.AcceptanceTest.Enviroment;
using DotNetRules.Tests.AcceptanceTest.Enviroment.Policy.Manual;
using Machine.Specifications;

namespace DotNetRules.Tests.AcceptanceTest
{
    [Subject("Executor")]
    class OrCombination
    {
        class When_Policy_Is_Specified_Explicitly_And_Policy_Tests_For_Matching_Or_Condition
        {
            static ExecutionTrace _result;
            static LegacyItem _legacyItem;
            static TargetDomainObject _targetDomainObject;

            Establish context = () =>
            {
                _legacyItem = new LegacyItem { Number = "a", Text = "" };
                _targetDomainObject = new TargetDomainObject { StringArray = new string[0], Integer = 0 };
            };

            Because of = () => _result = _legacyItem.ApplyPoliciesFor(_targetDomainObject, policies: new[] { typeof(PolicyWithOrThatSetsSourceNumberToOne) });

            It should_have_executed_the_policy_once = () => _result.Called.ShouldEqual(1);

            It should_have_set_the_policy_number_to_one = () => _legacyItem.Number.ShouldEqual("1");
        }

        class When_Policy_Is_Specified_Explicitly_And_Policy_Tests_For_NonMatching_Condition
        {
            static ExecutionTrace _result;
            static LegacyItem _legacyItem;
            static TargetDomainObject _targetDomainObject;

            Establish context = () =>
            {
                _legacyItem = new LegacyItem { Number = "a", Text = "has" };
                _targetDomainObject = new TargetDomainObject { StringArray = new string[0], Integer = 0 };
            };

            Because of = () => _result = _legacyItem.ApplyPoliciesFor(_targetDomainObject, policies: new[] { typeof(PolicyWithOrThatSetsSourceNumberToOne) });

            It should_not_have_executed_the_policy = () => _result.Called.ShouldEqual(0);

            It policy_number_should_be_unchanged = () => _legacyItem.Number.ShouldEqual("a");
        }
    }
}
