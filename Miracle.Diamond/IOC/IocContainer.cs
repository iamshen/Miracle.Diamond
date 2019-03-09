using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IOC
{
    public abstract class IocContainer<T> where T : class
    {
        private readonly ISet<object> g_persistent = null;

        private readonly IDictionary<Type, object> g_relational = null;

        public IocContainer()
        {
            g_persistent = new HashSet<object>();
            g_relational = new Dictionary<Type, object>();
        }

        public static bool Invalid(Type type)
        {
            if (type == null || type.IsValueType)
            {
                return false;
            }

            return !typeof(IServiceProvider).IsAssignableFrom(type);
        }

        private object Find(Type type)
        {
            lock (this)
            {
                object persistent = null;
                if (type == null)
                    return null;
                if (g_relational.TryGetValue(type, out persistent))
                    return persistent;
                persistent = g_persistent.FirstOrDefault(x => type.IsInstanceOfType(x));
                if (persistent != null)
                    g_relational.Add(type, persistent);
                return persistent;
            }
        }

        private Type FindInheritedType(Type type, Assembly[] assemblies)
        {
            if (type.IsInterface || type.IsAbstract)
            {
                Type inherited = assemblies.Select(x => x.GetTypes().FirstOrDefault(o => o.IsSubclassOf(type))).FirstOrDefault(k => k != null);
                return inherited;
            }
            return type;
        }

        private object CreateObject(Type type, Assembly[] assemblies)
        {
            lock (this)
            {
                ConstructorInfo ctor = type.GetConstructors().FirstOrDefault();
                if (ctor == null)
                    return null;
                ParameterInfo[] parameters = ctor.GetParameters();
                if (!parameters.Any())
                {
                    object obj = ctor.Invoke(null);
                    g_persistent.Add(obj);

                    return obj;
                }
                else
                {
                    object[] args = new object[parameters.Length];
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        ParameterInfo parameter = parameters[i];
                        Type inherited = FindInheritedType(parameter.ParameterType, assemblies);
                        if (inherited == null)
                            inherited = parameter.ParameterType;
                        object push = Find(inherited);
                        if (push == null)
                            push = CreateObject(inherited, assemblies);
                        if (push == null)
                            return push;
                        args[i] = push;
                    }

                    object obj = ctor.Invoke(args);
                    g_persistent.Add(obj);

                    return obj;
                }
            }
        }

        public virtual bool Register(object obj)
        {
            if (obj == null)
                throw new ArgumentException();
            lock (this)
            {
                return g_persistent.Add(obj);
            }
        }

        public virtual bool Register(Type type, Assembly[] assemblies)
        {
            return Resolve(type, assemblies) != null;
        }

        public virtual bool Register(Type type)
        {
            if (type != null)
                throw new ArgumentNullException();
            return Register(type, new[] { type.Assembly });
        }

        public virtual bool Register<T>()
        {
            return Register(typeof(T));
        }

        public virtual bool Register<T>(Assembly[] assemblies)
        {
            return Register(typeof(T), assemblies);
        }

        public virtual object Get(Type type)
        {
            if (type == null)
                throw new ArgumentNullException();
            return Find(type);
        }

        public virtual T Get<T>()
        {
            object obj = Get(typeof(T));
            if (obj == null)
                return default(T);
            return (T)obj;
        }

        public virtual void Load(Assembly assembly)
        {
            Load(new Assembly[] { assembly });
        }

        public virtual void Load(Assembly[] assemblies)
        {
            lock (this)
            {
                CheckAssmeblyAndThrowEmpty(assemblies);
                foreach (Assembly assembly in assemblies)
                {
                    foreach (Type type in assembly.GetExportedTypes())
                    {
                        if (!typeof(T).IsAssignableFrom(type))
                            continue;
                        Resolve(type, assemblies);
                    }
                }
            }
        }

        protected virtual object Resolve(Type type, Assembly[] assemblies)
        {
            CheckAssmeblyAndThrowEmpty(assemblies);
            if (type == null)
                throw new ArgumentNullException();
            if (!typeof(T).IsAssignableFrom(type))
                return new ArgumentException();
            lock (this)
            {
                object obj = Find(type);
                if (obj == null)
                    obj = CreateObject(type, assemblies);

                return obj;
            }
        }

        public void CheckAssmeblyAndThrowEmpty(Assembly[] assemblies)
        {
            if (assemblies == null)
                throw new ArgumentNullException();
            if (assemblies.Length == 0)
                throw new ArgumentException();
            if (assemblies.Where(x => x == null).Any())
                throw new ArgumentNullException();
        }
    }
}
