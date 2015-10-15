using System;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;

namespace RhinoMocks.FromInstance
{
    public static class FromInstanceExtension
    {
        /// <summary>
        /// <see cref="RhinoMocksExtensions.Stub{T}"/>s all members in type <typeparamref name="T"/> on
        /// <paramref name="mock"/> so that they <see cref="IMethodOptions{T}.Do"/>
        /// against <paramref name="instance"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// MockRepository
        ///     .GenerateMock<IFoo>()
        ///     .StubFromInstance(new Foo()); 
        ///  ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// It is necessary to add new <see cref="RhinoMocksExtensions.Stub{T}"/>s BEFORE
        /// calling <see cref="StubFromInstance{T}"/>.  Calling <see cref="RhinoMocksExtensions.Stub{T}"/>
        /// after, will be ignored.
        /// </remarks>
        /// <returns>
        /// Returns <paramref name="mock"/>.
        /// </returns>
        public static T StubFromInstance<T>(this T mock, T instance)
            where T : class
        {
            throw new NotImplementedException();
        }
    }
}
