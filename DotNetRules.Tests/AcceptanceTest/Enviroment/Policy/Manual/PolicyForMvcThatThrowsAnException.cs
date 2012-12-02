using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetRules.Runtime;

namespace DotNetRules.Tests.AcceptanceTest.Enviroment.Policy.Manual
{
    [Policy(typeof(ExampleSourceObject), AutoExecute = false)]
    class PolicyForMvcThatThrowsAnException
    {
        Given that = () => true;

        Then ModelProperty = () => { throw new Exception("incorrect state"); };
    }
}
