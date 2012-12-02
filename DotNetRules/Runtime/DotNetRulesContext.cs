using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotNetRules.Runtime
{
    class DotNetRulesContext
    {
        readonly object _policy;
        readonly Establish _establishClause;
        readonly Finally _finally;
        private readonly IEnumerable<Given> _givenClauses;
        private readonly IEnumerable<Or> _orClauses;
        private readonly IEnumerable<Then> _thenClauses;
        bool _wasGiven;

        public Type CurrentPolicy { get { return _policy.GetType(); } }

        public bool CanReturn { get { return CurrentPolicy.ReturnField().Any(); } }

        public DotNetRulesContext(Type policy)
        {
            if (policy == null) throw new ArgumentNullException("policy");
            var constructorInfo = policy.GetConstructor(Type.EmptyTypes);
            if (constructorInfo == null)
                throw new Exception(string.Format("Policy {0} needs constructor without any arguments", policy));
            _policy = constructorInfo.Invoke(null);
            _establishClause = Helper.GetFieldValuesFor<Establish>(_policy, policy.GetInstanceFieldsOfType<Establish>()).FirstOrDefault();
            _givenClauses = Helper.GetFieldValuesFor<Given>(_policy, policy.GetInstanceFieldsOfType<Given>());
            _orClauses = Helper.GetFieldValuesFor<Or>(_policy, policy.GetInstanceFieldsOfType<Or>());
            _thenClauses = Helper.GetFieldValuesFor<Then>(_policy, policy.GetInstanceFieldsOfType<Then>());
            _finally = Helper.GetFieldValuesFor<Finally>(_policy, policy.GetInstanceFieldsOfType<Finally>()).FirstOrDefault();
        }
        
        public void Establish<T>(T target)
        {
            foreach (var property in GetProperties<T>())
            {
                property.SetValue(_policy, target, null);
            }

            if (_establishClause != null)
            {
                _establishClause(target);
            }
        }

        public void Establish<TSource, TTarget>(TSource source, TTarget target)
        {
            foreach (var property in GetProperties<TSource>())
            {
                property.SetValue(_policy, source, null);
            }

            foreach (var property in GetProperties<TTarget>())
            {
                property.SetValue(_policy, target, null);
            }

            if (_establishClause != null)
            {
                _establishClause(target);
            }
        }

        public void EstablishMore(IEnumerable<dynamic> sources)
        {
            foreach (var source in sources)
            {
                foreach (var property in GetProperties(source.GetType()))
                {
                    property.SetValue(_policy, source, null);
                }
            }

            if (_establishClause != null)
            {
                _establishClause(sources);
            }
        }
        
        public bool Given()
        {
            _wasGiven = _givenClauses.InvokeAll();
            return _wasGiven;
        }

        public bool Or()
        {
            return !_orClauses.Any() ? _wasGiven : _orClauses.InvokeAll();
        }

        public IEnumerable<ExceptionInformation> Then(bool catchException)
        {
            return _thenClauses.InvokeAll(catchException);
        }

        public void Finally()
        {
            if (_finally != null)
            {
                _finally.Invoke();
            }
        }

        public T Return<T>()
        {
            var @return = Helper.GetFieldValuesFor<Return<T>>(_policy, _policy.GetType().GetInstanceFieldsOfType<Return<T>>()).FirstOrDefault();
            return (@return != null) ? @return.Invoke() : DefaultReturn<T>();
        }

        T DefaultReturn<T>()
        {
            foreach (var property in GetProperties<T>())
            {
                return (T) property.GetValue(_policy, null);
            }

            return default(T);
        }

        public dynamic Return(Type type)
        {
            return GetProperties(type).Select(property => property.GetValue(_policy, null)).FirstOrDefault();
        }

        IEnumerable<PropertyInfo> GetProperties<T>()
        {
            return _policy.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy).Where(property => property.PropertyType == typeof(T));
        }

        IEnumerable<PropertyInfo> GetProperties(Type t)
        {
            return _policy.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy).Where(property => property.PropertyType == t);
        }
    }
}
