using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace DotNetRules.Runtime
{
    public static class Helper
    {
        public static IEnumerable<T> GetFieldValuesFor<T>(object instance, IEnumerable<FieldInfo> fields)
        {
            var contextClauses = ExtractPrivateFieldValues<T>(instance);
            contextClauses.Reverse();
            return contextClauses;
        }

        public static IEnumerable<FieldInfo> ReturnField(this Type type)
        {
            return GetInstanceFields(type).Where(fieldInfo => fieldInfo.Name.Contains("Return"));
        }

        public static IEnumerable<FieldInfo> GetInstanceFields(this Type type)
        {
            return type.GetFields(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public);
        }

        public static IEnumerable<FieldInfo> GetInstanceFieldsOfType<T>(this Type type)
        {
            var targetType = typeof (T);
            return GetInstanceFields(type).Where(fieldInfo => fieldInfo.FieldType == targetType);
        }

        static readonly Dictionary<string, Queue<Type>> Cache = new Dictionary<string, Queue<Type>>();

        public static void InvalidateCache()
        {
            Cache.Clear();
        }
        
        public static Queue<Type> GetTypesWithPolicyAttribute(this Assembly assembly, bool onlyIfSpecified = false, params Type[] target)
        {
            var cacheKey = string.Concat(assembly.FullName, onlyIfSpecified, string.Join("-", target.Select(_ => _.FullName)));

            if (Cache.ContainsKey(cacheKey))
            {
                return Cache[cacheKey];
            }

            var queueTypes = new Queue<Type>();
            try
            {
                var types = assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(PolicyAttribute), true).Length > 0);
                var unBlockedTypeSets = new List<Type>();
                var blockedTypeSets = new Dictionary<PolicyAttribute, Type>();
                foreach (var type in types)
                {
                    var policyAttribute = (PolicyAttribute)type.GetCustomAttributes(typeof(PolicyAttribute), true).FirstOrDefault();
                    if (policyAttribute == null || (policyAttribute.Types != null && (!target.All(policyAttribute.Types.Contains) || !policyAttribute.Types.All(target.Contains)))) continue;
                    if (!policyAttribute.AutoExecute && !onlyIfSpecified) continue;

                    if (policyAttribute.WaitFor == null)
                    {
                        unBlockedTypeSets.Add(type);
                    }
                    else
                    {
                        blockedTypeSets.Add(policyAttribute, type);
                    }
                }
                unBlockedTypeSets.OrderBy(_ => _.Name);
                foreach (var item in unBlockedTypeSets)
                {
                    queueTypes.Enqueue(item);
                    var store = item;
                    foreach (var type in blockedTypeSets.Where(_ => _.Key.WaitFor == store))
                    {
                        queueTypes.Enqueue(type.Value);
                    }
                }
            }
            catch
            {
            }

            Cache.Add(cacheKey, queueTypes);
            return queueTypes;
        }

        public static bool InvokeAll(this IEnumerable<Or> invokeable)
        {
            return invokeable.AllNonNull().Select<Or, Func<bool>>(x => x.Invoke).InvokeAll();
        }

        public static bool InvokeAll(this IEnumerable<Given> invokeable)
        {
            return invokeable.AllNonNull().Select<Given, Func<bool>>(x => x.Invoke).InvokeAll();
        }

        public static void InvokeAll(this IEnumerable<Then> invokeable)
        {
            invokeable.AllNonNull().Select<Then, Action>(x => x.Invoke).InvokeAll();
        }

        public static IEnumerable<ExceptionInformation> InvokeAll(this IEnumerable<Then> invokeable, bool catchErrors)
        {
            if (!catchErrors)
            {
                invokeable.InvokeAll();
                return new ExceptionInformation[0];
            }
            var errors = new List<ExceptionInformation>();
            invokeable.AllNonNull().Select<Then, Action>(x => delegate {
                try
                {
                    x.Invoke();
                }
                catch (Exception e0)
                {
                    SourceAttribute name;
                    errors.Add(new ExceptionInformation { 
                        Exception = e0, 
                        Message = e0.Message, 
                        Source = (name = (SourceAttribute)x.Method.GetCustomAttributes(typeof(SourceAttribute), true).FirstOrDefault()) == null ?
                            x.Method.Name: 
                            name.Name});
                } 
            }).InvokeAll();
            return errors;
        }
        
        public static void Each<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var t in enumerable)
            {
                action(t);
            }
        }
        
        static List<T> ExtractPrivateFieldValues<T>(object instance)
        {
            var delegates = new List<T>();
            var type = instance.GetType();
            CollectDetailsOf(type, () => instance, delegates);

            return delegates;
        }

        static void CollectDetailsOf<T>(
            Type target,
            Func<object> instanceResolver,
            ICollection<T> items)
        {
            if (target == typeof(object) || target == null)
            {
                return;
            }

            if (!target.IsAbstract && (!target.IsAbstract || !target.IsSealed))
            {
                var instance = instanceResolver();
                if (instance == null)
                {
                    return;
                }

                var fields = target.GetInstanceFieldsOfType<T>();

                foreach (var val in from field in fields where field != null select new { value = (T)field.GetValue(instance), name = field.Name })
                {
                    var @delegate = val.value as Delegate;
                    if (@delegate != null)
                    {
                        var method = @delegate.Method;
                        var fieldInfo = method.GetType().GetField("m_name",
                                                                  BindingFlags.NonPublic |
                                                                  BindingFlags.Instance);
                        if (fieldInfo != null)
                            fieldInfo.SetValue(method, val.name);
                        Debug.WriteLine(method);
                    }
                    items.Add(val.value);
                }

                CollectDetailsOf(target.BaseType, () => instance, items);
            }

            CollectDetailsOf(target.DeclaringType, () => target.DeclaringType != null ? Activator.CreateInstance(target.DeclaringType) : null, items);
        }

        static IEnumerable<T> AllNonNull<T>(this IEnumerable<T> elements) where T : class
        {
            return elements.Where(x => x != null);
        }

        static void InvokeAll(this IEnumerable<Action> actions)
        {
            actions.Each(x => x());
        }

        static bool InvokeAll(this IEnumerable<Func<bool>> actions)
        {
            return actions.All(action => action());
        }
    }
}
