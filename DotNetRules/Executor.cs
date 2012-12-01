using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetRules.Runtime;

namespace DotNetRules
{
    public class Settings
    {
        public bool CatchExceptions { get; set; }
    }

    public static class Executor
    {
        public static readonly Settings Settings  = new Settings { CatchExceptions = false };

        public static List<Exception> Exceptions { get; private set; }

        
        public static ExecutionTrace ApplyPolicies<TSubject>(this TSubject subject, Assembly policyLocation = null, IEnumerable<Type> policies = null)
        {
            return Apply(subject, policyLocation, policies);
        }

        public static ExecutionTrace ApplyPoliciesFor<TSubject, TSource>(this TSubject subject, TSource source, Assembly policyLocation = null, IEnumerable<Type> policies = null)
        {
            return (ExecutionTrace)Apply<object, TSubject, TSource>(subject, source, policyLocation, policies);
        }

        public static ExecutionTrace Apply<TSubject>(TSubject subject, Assembly policyLocation = null, IEnumerable<Type> policies = null)
        {
            return (ExecutionTrace)Apply<object, TSubject>(subject, policyLocation, policies);
        }

        public static ExecutionTrace<TReturn> ApplyPolicies<TReturn, TSubject>(this TSubject subject, Assembly policyLocation = null, IEnumerable<Type> policies = null)
        {
            return Apply<TReturn, TSubject>(subject, policyLocation, policies);
        }

        public static ExecutionTrace<TReturn> ApplyPoliciesFor<TReturn, TSubject, TSource>(this TSubject subject, TSource source, Assembly policyLocation = null, IEnumerable<Type> policies = null)
        {
            return Apply<TReturn, TSubject, TSource>(subject, source, policyLocation, policies);
        }

        public static ExecutionTrace<TReturn> Apply<TReturn, TSubject>(TSubject subject, Assembly policyLocation = null, IEnumerable<Type> policies = null)
        {
            if (policies == null)
            {
                policies = Enumerable.Empty<Type>();
            }
            var type = (typeof(TSubject));
            if (policyLocation == null)
            {
                policyLocation = type.Assembly;
            }
            if (Settings.CatchExceptions)
            {
                Exceptions = new List<Exception>();
            }

            var executionTrace = new ExecutionTrace<TReturn>(policyLocation);
            foreach (var mon in policyLocation.GetTypesWithPolicyAttribute(policies.Any(), type)
                .Select(item => new DotNetRulesContext(item))
                .Where(_ => (!policies.Any()) || policies.Any(type1 => type1 == _.CurrentPolicy)))
            {
                mon.Establish(subject);
                if (mon.Given() || mon.Or())
                {
                    executionTrace.Called++;
                    executionTrace.By.Enqueue(mon.CurrentPolicy);

                    MonitorThen(mon);
                }
                mon.Finally();
                executionTrace.ReturnType = mon.Return<TReturn>();
            }
            return executionTrace;
        }

        public static ExecutionTrace Apply<TSource, TTarget>(TSource source, TTarget target, Assembly policyLocation = null, IEnumerable<Type> policies = null)
        {
            return (ExecutionTrace)Apply<object, TSource, TTarget>(source, target, policyLocation, policies);
        }

        public static ExecutionTrace<TReturn> Apply<TReturn, TSource, TTarget>(TSource source, TTarget target, Assembly policyLocation = null, IEnumerable<Type> policies = null)
        {
            if (policies == null)
            {
                policies = Enumerable.Empty<Type>();
            }
            var type = (typeof(TSource));
            if (policyLocation == null)
            {
                policyLocation = type.Assembly;
            }
            if (Settings.CatchExceptions)
            {
                Exceptions = new List<Exception>();
            }

            var executionTrace = new ExecutionTrace<TReturn>(policyLocation);
            foreach (var mon in policyLocation.GetTypesWithPolicyAttribute(policies.Any(), type, typeof(TTarget))
                .Select(item => new DotNetRulesContext(item))
                .Where(_ => !policies.Any() || policies.Any(type1 => type1 == _.CurrentPolicy)))
            {
                mon.Establish(source, target);
                if (mon.Given() || mon.Or())
                {
                    executionTrace.Called++;
                    executionTrace.By.Enqueue(mon.CurrentPolicy);

                    MonitorThen(mon);
                }
                mon.Finally();
                executionTrace.ReturnType = mon.Return<TReturn>();
            }
            return executionTrace;
        }
        
        public static ExecutionTrace Apply(params dynamic[] values)
        {
            return Apply(null, null, values);
        }

        public static ExecutionTrace Apply(IEnumerable<Type> policies, params dynamic[] values)
        {
            return Apply(null, policies, values);
        }

        public static ExecutionTrace Apply(Assembly policyLocation, params dynamic[] values)
        {
            return Apply(null, null, values);
        }

        public static ExecutionTrace Apply(Assembly policyLocation, IEnumerable<Type> policies, params dynamic[] values)
        {
            return (ExecutionTrace)Apply<object>(policyLocation, policies, values);
        }
        
        public static ExecutionTrace<TReturn> Apply<TReturn>(params dynamic[] values)
        {
            return Apply<TReturn>(null, null, values);
        }

        public static ExecutionTrace<TReturn> Apply<TReturn>(IEnumerable<Type> policies, params dynamic[] values)
        {
            return Apply<TReturn>(null, policies, values);
        }

        public static ExecutionTrace<TReturn> Apply<TReturn>(Assembly policyLocation, params dynamic[] values)
        {
            return Apply<TReturn>(null, null, values);
        }

        public static ExecutionTrace<TReturn> Apply<TReturn>(Assembly policyLocation, IEnumerable<Type> policies, params dynamic[] values)
        {
            if (policies == null)
            {
                policies = Enumerable.Empty<Type>();
            }
            var types = values.Select(_ => (Type)_.GetType()).ToArray();
            if (policyLocation == null)
            {
                policyLocation = types.First().Assembly;
            }
            if (Settings.CatchExceptions)
            {
                Exceptions = new List<Exception>();
            }

            var target = new ExecutionTrace<TReturn>(policyLocation);
            foreach (var mon in policyLocation.GetTypesWithPolicyAttribute(policies.Any(), types.ToArray())
                .Select(item => new DotNetRulesContext(item))
                .Where(_ => !policies.Any() || policies.Any(type1 => type1 == _.CurrentPolicy)))
            {
                mon.EstablishMore(values);
                if (mon.Given() || mon.Or())
                {
                    target.Called++;
                    target.By.Enqueue(mon.CurrentPolicy);

                    MonitorThen(mon);
                }
                mon.Finally();
                for (var i = 0; i < values.Count(); i++)
                {
                    var value = values[i];
                    target.ReturnType = mon.Return<TReturn>();
                }
            }
            return target;
        }

        static void MonitorThen(DotNetRulesContext mon)
        {
            if (Settings.CatchExceptions)
            {
                try
                {
                    mon.Then();
                }
                catch (Exception e0)
                {
                    Exceptions.Add(e0);
                }
            }
            else
            {
                mon.Then();
            }
        }
    }
}
