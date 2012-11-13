using System;
using System.Collections.Generic;
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

        public static Queue<Type> GetTypesWithPolicyAttribute(this Assembly assembly, bool selected = false, params Type[] target)
        {
            var types = assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(PolicyAttribute), true).Length > 0);
            var queueTypes = new Queue<Type>();
            var unBlockedTypeSets = new List<Type>();
            var blockedTypeSets = new Dictionary<PolicyAttribute, Type>();
            foreach (var type in types)
            {
                var policyAttribute = (PolicyAttribute) type.GetCustomAttributes(typeof (PolicyAttribute), true).FirstOrDefault();
                if (policyAttribute == null || (policyAttribute.Types != null && (!target.All(policyAttribute.Types.Contains) || !policyAttribute.Types.All(target.Contains)))) continue;
                if (!policyAttribute.AutoExecute && !selected) continue;

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

                foreach (var val in from field in fields where field != null select (T)field.GetValue(instance))
                {
                    items.Add(val);
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
