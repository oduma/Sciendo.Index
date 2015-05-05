using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sciendo.IOC
{
    public class AssemblyScanner
    {
        private Assembly[] _assemblies;

        public AssemblyScanner From(params Assembly[] inAssemblies)
        {
            _assemblies = inAssemblies;
            return this;
        }

        public IEnumerable<RegisteredType> BasedOn<T>()
        {
            foreach(var assembly in _assemblies)
            {
                foreach(var type in assembly.GetTypes().Where(t=>t.IsClass && !t.IsAbstract && typeof(T).IsAssignableFrom(t)))
                {
                     yield return  new RegisteredType { Implementation = type, Service = typeof(T), Name = typeof(T).FullName };
                }
            }
        }

    }
}
