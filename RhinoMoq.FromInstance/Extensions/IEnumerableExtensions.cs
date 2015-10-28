using System.Collections.Generic;
using System.Linq;

namespace RhinoMoq.FromInstance.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Concat<T>(this T obj, IEnumerable<T> additionalItems)
        {
            return new T[] {obj}.Concat(additionalItems);
        }
    }
}
