using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetRules.Tests.AcceptanceTest.Enviroment;
using DotNetRules.Tests.AcceptanceTest.Enviroment.Policy;
using Machine.Specifications;

namespace DotNetRules.Tests.AcceptanceTest
{
    [Subject("Executor")]
    class OrCombination
    {
        class When_Policy_Is_Specified_Explicitly_And_Policy_Tests_For_Or_Condition
        {
            static ExecutionTrace _result;
            static LegacyItem _legacyItem;
            static TargetDomainObject _targetDomainObject;

            Establish context = () =>
            {
                _legacyItem = new LegacyItem { Number = "a", Text = ""};
                _targetDomainObject = new TargetDomainObject { StringArray = new string[0], Integer = 0 };
            };

            Because of = () => _result = _legacyItem.ApplyPoliciesFor(_targetDomainObject, policies: new[] { typeof(PolicyWithOr) });

            It should_have_executed_the_policy_once = () => _result.Called.ShouldEqual(1);

            It should_have_set_the_policy_number_to_one = () => _legacyItem.Number.ShouldEqual("1");
        }
    }
}
