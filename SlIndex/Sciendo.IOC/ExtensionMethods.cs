using System.Collections.Generic;

namespace Sciendo.IOC
{
    public static class ExtensionMethods
    {
        public static IEnumerable<RegisteredType> With(this IEnumerable<RegisteredType> inTypes, LifeStyle lifeStyle)
        {
            foreach(var inType in inTypes)
            {
                yield return inType.With(lifeStyle);
            }
        }
    }
}
