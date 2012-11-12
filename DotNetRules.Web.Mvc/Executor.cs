using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;

namespace DotNetRules.Web.Mvc
{
    public static class MvcExecutor
    {
        public static ExecutionTrace ApplyPolicies<TSubject>(this ModelStateDictionary modelState, TSubject subject, Assembly policyLocation = null, IEnumerable<Type> policies = null)
        {
            Executor.Settings.CatchExceptions = true;
            var applyPoliciesFor = Executor.Apply(subject, policyLocation, policies);
            Executor.Settings.CatchExceptions = false;
            foreach (var exception in Executor.Exceptions)
            {
                modelState.AddModelError("", exception.Message);
            }
            return applyPoliciesFor;
        }

        public static ExecutionTrace ApplyPoliciesFor<TSource, TTarget>(this ModelStateDictionary modelState, TTarget target, TSource source, Assembly policyLocation = null, IEnumerable<Type> policies = null)
        {
            Executor.Settings.CatchExceptions = true;
            var applyPoliciesFor = Executor.Apply(target, source, policyLocation, policies);
            Executor.Settings.CatchExceptions = false;
            foreach (var exception in Executor.Exceptions)
            {
                modelState.AddModelError("", exception.Message);
            }
            return applyPoliciesFor;
        }
    }
}
