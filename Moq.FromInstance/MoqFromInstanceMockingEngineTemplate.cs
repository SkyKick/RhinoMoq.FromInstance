using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Moq.Language.Flow;
using RhinoMoq.FromInstance;
using RhinoMoq.FromInstance.Extensions;

namespace Moq.FromInstance
{
    /// <summary>
    /// Moq specific <see cref="IFromInstanceMockingEngineTemplate"/>, which adds support
    /// for Moq to <see cref="FromInstanceMockingEngine"/>.
    /// </summary>
    public class MoqFromInstanceMockingEngineTemplate : IFromInstanceMockingEngineTemplate
    {
        public Type BuildSetupMethodFunctionType(Type mockTargetType, Type mockedMethodReturnType)
        {
            return 
                (mockedMethodReturnType == typeof(void))
                ? typeof(Action<>)
                    .MakeGenericType(
                        mockTargetType)
                : typeof(Func<,>)
                    .MakeGenericType(
                        mockTargetType, 
                        mockedMethodReturnType);
        }

        public object ExecuteSetupMethod(
            Type mockTargetType, Type mockedMethodReturnType,
            object mock, LambdaExpression setupExpression)
        {
            var setupMethod =

                (typeof(void) == mockedMethodReturnType)
                ? typeof(Mock)
                    .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                    .First(x => x.Name == "Setup" && x.GetGenericArguments().Length == 1)
                    .MakeGenericMethod(
                        mockTargetType)
                : typeof(Mock)
                    .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                    .First(x => x.Name == "Setup" && x.GetGenericArguments().Length == 2)
                    .MakeGenericMethod(
                        mockTargetType,
                        mockedMethodReturnType);

            return setupMethod
                .Invoke(
                    null,
                    BindingFlags.NonPublic | BindingFlags.Static,
                    null,
                    new object[] { mock, setupExpression, null },
                    CultureInfo.CurrentCulture);
        }

        public MethodInfo BuildReturnsMethod(Type mockTargetType, MethodInfo mockedMethod, object mock, object instance)
        {
            var methodCallReturnType =
                typeof (ISetup<,>).Assembly
                    .GetTypes()
                    .First(t => t.Name == "MethodCallReturn`2")
                    .MakeGenericType(mockTargetType, mockedMethod.ReturnType);

            if (mockedMethod.GetParameters().Length == 0)
            {
                return
                    methodCallReturnType
                        .GetMethod(
                            "Returns",
                            new[]
                            {
                                typeof (Func<>).MakeGenericType(mockedMethod.ReturnType)
                            });

            }
            else
            {
                //Note: This version of Returns has a signature
                //Returns<TParam1, TParam2, TReturn> but TReturn is already
                //set in the MethodCallReturn (parent object).

                return 
                    methodCallReturnType
                        .GetMethods()
                        .First(x => 
                            x.Name == "Returns" &&
                            x.GetGenericArguments().Length == mockedMethod.GetParameters().Length)
                        .MakeGenericMethod(
                            mockedMethod.GetParameters()
                                .Select(x => x.ParameterType)
                                .ToArray());
            }
        }

        public object BuildArgsTemplate(Type mockedMethodParameterType)
        {
           return
                typeof (It)
                    .GetMethod("IsAny", BindingFlags.Public | BindingFlags.Static)
                    .MakeGenericMethod(mockedMethodParameterType)
                    .Invoke(null, null);
        }
    }
}
