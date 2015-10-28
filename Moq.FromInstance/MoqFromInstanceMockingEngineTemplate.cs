using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Moq.Language.Flow;
using RhinoMoq.FromInstance;

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
            object mock, object setupExpression)
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

        public MethodInfo BuildReturnsMethod(Type mockTargetType, Type mockedMethodReturnType, object mock, object instance)
        {
            var methodCallReturnType =
                   typeof(ISetup<,>).Assembly
                       .GetTypes()
                       .First(t => t.Name == "MethodCallReturn`2");

            return
                methodCallReturnType
                    .MakeGenericType(mockTargetType, mockedMethodReturnType)
                    .GetMethod(
                        "Returns",
                        new[]
                        {
                            typeof(Func<>).MakeGenericType(mockedMethodReturnType)
                        });
        }

        public object BuildArgsTemplate(Type mockedMethodParameterType)
        {
            throw new NotImplementedException();
        }
    }
}
