using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace RhinoMoq.FromInstance
{
    public interface IFromInstanceMockingEngineTemplate
    {
        Type BuildSetupMethodFunctionType(Type mockTargetType, Type mockedMethodReturnType);

        object ExecuteSetupMethod(Type mockTargetType, Type mockedMethodReturnType,
            object mock, object setupExpression);

        [NotNull]
        MethodInfo BuildReturnsMethod(Type mockTargetType, Type mockedMethodReturnType, object mock, object instance);
    }

    public class FromInstanceMockingEngine
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mock"></param>
        /// <param name="instance"></param>
        /// <param name="template"></param>
        /// <exception cref="UnsupportedInstanceTypeException">
        /// Thrown if <typeparamref name="T"/> is not an interface.
        /// </exception>
        public void MockFromInstance<T>(object mock, T instance, IFromInstanceMockingEngineTemplate template)
        {
            if (!typeof (T).IsInterface)
                throw new UnsupportedInstanceTypeException(typeof (T));

            var mockedMembers = GetMembersToMock<T>();

            foreach (var member in mockedMembers)
            {
                if (member.ReturnType == typeof (void))
                    MockVoidMethod<T>(member, mock, template);

                else
                    MockNonVoidMethod(member, mock, instance, template);
            }
        }
        
        private IEnumerable<MethodInfo> GetMembersToMock<T>()
        {
            return
                //methods
                typeof(T).GetMethods()
                    //properties
                    .Union(
                        typeof(T).GetProperties()
                            .SelectMany(pi =>
                            {
                                var propertyMethods = new List<MethodInfo>();

                                if (pi.CanRead)
                                    propertyMethods.Add(pi.GetGetMethod());

                                if (pi.CanWrite)
                                    propertyMethods.Add(pi.GetSetMethod());

                                return propertyMethods;
                            }));
        }

        private void MockVoidMethod<T>(MethodInfo member, object mock, IFromInstanceMockingEngineTemplate template)
        {
            CreateAndInvokeSetupMethod<T>(member, mock, template);
        }

        private void MockNonVoidMethod<T>(MethodInfo member, object mock, T instance, IFromInstanceMockingEngineTemplate template)
        {
            var setupResponse = CreateAndInvokeSetupMethod<T>(member, mock, template);

            CreateAndInvokeReturnsMethod(setupResponse, member, mock, instance, template);
        }

        /// <summary>
        /// Creates and Invokes the "Setup" method and returns the result as an object.
        /// Actual return type is Mocking Framework specific.
        /// </summary>
        private object CreateAndInvokeSetupMethod<T>(MethodInfo method, object mock, IFromInstanceMockingEngineTemplate template)
        {
            var setupExpressionLambdaParameter = Expression.Parameter(typeof (T), "x");

            var setupExpression =
                    Expression.Lambda(
                        delegateType:
                            template.BuildSetupMethodFunctionType(typeof(T), method.ReturnType),
                        body:
                            Expression.Property(
                                setupExpressionLambdaParameter,
                                method),
                        parameters:
                            new List<ParameterExpression>
                            {
                                setupExpressionLambdaParameter
                            });

            return template.ExecuteSetupMethod(
                typeof (T),
                method.ReturnType,
                mock,
                setupExpression);
        }

        private void CreateAndInvokeReturnsMethod<T>(
            object setupResponse, MethodInfo member, object mock, T instance,
            IFromInstanceMockingEngineTemplate template)
        {
            var returnsMethod = 
                template.BuildReturnsMethod(
                    member.ReturnType, 
                    typeof(T),
                    mock, 
                    instance);

            var returnsFunction =
                Expression.Lambda(
                    typeof(Func<>)
                        .MakeGenericType(member.ReturnType),
                    Expression.Convert(
                        Expression.Call(
                            Expression.Constant(
                                instance),
                            member),
                        member.ReturnType))
                    .Compile();

            returnsMethod
                .Invoke(
                    setupResponse,
                    new object[]
                    {
                        //DelegateWrapper.Create(returnsFunction)
                        returnsFunction
                    });
        }
    }
}
