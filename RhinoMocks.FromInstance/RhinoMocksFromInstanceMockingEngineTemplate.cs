using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Remoting;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using Rhino.Mocks.Impl;
using RhinoMoq.FromInstance;

namespace RhinoMocks.FromInstance
{
    /// <summary>
    /// Rhino Mocks specific <see cref="IFromInstanceMockingEngineTemplate"/>, which adds support
    /// for Rhino Mocks to <see cref="FromInstanceMockingEngine"/>.
    /// </summary>
    public class RhinoMocksFromInstanceMockingEngineTemplate : IFromInstanceMockingEngineTemplate
    {
        public Type BuildSetupMethodFunctionType(Type mockTargetType, Type mockedMethodReturnType)
        {
            return
               (mockedMethodReturnType == typeof(void))
               ? typeof(Action<>)
                   .MakeGenericType(
                       mockTargetType)
               : typeof(Function<,>)
                   .MakeGenericType(
                       mockTargetType,
                       mockedMethodReturnType);
        }

        public object ExecuteSetupMethod(
            Type mockTargetType, 
            Type mockedMethodReturnType, 
            object mock, 
            LambdaExpression setupExpression)
        {
            var setupMethod =
                typeof(RhinoMocksExtensions)
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .Where(m => m.Name == "Stub")
                    .ElementAt(1)
                    .MakeGenericMethod(
                        mockTargetType, mockedMethodReturnType);

            return setupMethod
                .Invoke(
                    null,
                    BindingFlags.NonPublic | BindingFlags.Static,
                    null,
                    new object[] { mock, setupExpression.Compile() },
                    CultureInfo.CurrentCulture);
        }

        public MethodInfo BuildReturnsMethod(Type mockTargetType, MethodInfo mockedMethodReturnType, object mock, object instance)
        {
            return
                typeof(MethodOptions<>)
                    .MakeGenericType(mockedMethodReturnType)
                    //backing store for Do method
                    .GetMethod("Do");
        }

        public object BuildArgsTemplate(Type mockedMethodParameterType)
        {
            var argTemplateType =
                typeof (Arg<>)
                    .MakeGenericType(mockedMethodParameterType);

            var argIs =
                argTemplateType
                    .GetProperty("Is", BindingFlags.Public | BindingFlags.Static)
                    .GetValue(null, null);
            
            return 
                argIs
                    .GetType()
                    .GetProperty("Anything")
                    .GetValue(argIs, null);
        }
    }
}
