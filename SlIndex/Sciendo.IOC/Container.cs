using System;
using System.Collections.Generic;
using System.Linq;

namespace Sciendo.IOC
{
    public class Container
    {
        private static object synchLock = new object();

        internal List<RegisteredType> RegisteredTypes
        {
            get;
            set;
        }

        private static readonly Container Instance= new Container(); 

        private Container()
        {
            lock (synchLock)
            {
                RegisteredTypes = (RegisteredTypes) ?? new List<RegisteredType>();
            }
        }

        public static Container GetInstance()
        {
            return Instance;
        }
        public void Add(params RegisteredType[] registeredTypes)
        {
            foreach(var registeredType in registeredTypes)
            {
                lock(synchLock)
                {
                    if (registeredType.Validate())
                    {
                        RegisteredTypes.Add(registeredType);
                    }
                }
            }
        }

        private static string GetKey(Type serviceType, string name)
        {
            var key = serviceType.FullName;
            if (!string.IsNullOrEmpty(name))
            {
                key = name;
            }
            return key;
        }

        public T[] ResolveAll<T>()
        {
            lock (synchLock)
            {
                return RegisteredTypes.Where(t => t.IsSuitable<T>()).Select(t => (T)t.Instance).ToArray();
            }
        }

        public T Resolve<T>(string name=null)
        {
            lock (synchLock)
            {
                string key = GetKey(typeof(T), name);
                var instance = RegisteredTypes.FirstOrDefault(t => t.IsSuitable<T>(key));
                if (instance == null)
                    throw new NotImplementedException("Cannot resolve " + typeof(T).FullName);
                return (T)instance.Instance;
            }
        }
    }
}
