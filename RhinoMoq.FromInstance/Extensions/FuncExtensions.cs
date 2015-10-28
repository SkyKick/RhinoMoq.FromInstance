using System;
using System.Linq;
using System.Reflection;

namespace RhinoMoq.FromInstance.Extensions
{
    public static class FuncExtensions
    {
        /// <summary>
        /// Creates an appropriate <see cref="Func{TResult}"/> <see cref="Type"/>
        /// to match  the <paramref name="method"/> signature to include the 
        /// <see cref="MethodInfo.ReturnType"/> and <see cref="MethodBase.GetParameters"/>.
        /// 
        /// This includes making the correct call to <see cref="Type.MakeGenericType"/>.
        /// </summary>
        public static Type GetFuncTypeForMethod(this MethodInfo method)
        {
            return
                typeof(Func<>)
                    .Assembly
                    .GetTypes()
                    .First(x =>
                        x.Name == "Func`" + (method.GetParameters().Length + 1))
                    //add correct generic type params
                    .MakeGenericType(
                        method
                            .GetParameters()
                            .Select(x => x.ParameterType)
                            .Concat(new [] {method.ReturnType})
                            .ToArray());
        }
    }
}
