using System;
using Moq.Language;
using RhinoMoq.FromInstance;

namespace Moq.FromInstance
{
    public static class FromInstanceExtension
    {
        /// <summary>
        /// <see cref="Mock{T}.Setup"/>s all members in type <typeparamref name="T"/> on
        /// <paramref name="mock"/> so that they <see cref="IReturns{TMock,TResult}.Returns(TResult)"/>
        /// against <paramref name="instance"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// new Mock<IFoo>()
        ///     .SetupFromInstance(new Foo()); 
        ///  ]]>
        /// </code>
        /// </example>
        /// <returns>
        /// Returns <paramref name="mock"/>.
        /// </returns>
        public static Mock<T> SetupFromInstance<T>(this Mock<T> mock, T instance)
            where T : class
        {
            new FromInstanceMockingEngine()
                .MockFromInstance<T>(
                    mock, 
                    instance, 
                    new MoqFromInstanceMockingEngineTemplate());

            return mock;
        }
    }
}
