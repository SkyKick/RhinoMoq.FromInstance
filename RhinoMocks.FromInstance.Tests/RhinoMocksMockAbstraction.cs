using System;
using Rhino.Mocks;
using RhinoMoq.FromInstance.Tests.TemplateExtensionTests;

namespace RhinoMocks.FromInstance.Tests
{
    public class RhinoMocksMockAbstraction : IMockAbstraction
    {
        public T CreateMockAndSetupFromInstance<T>(
            T actual,
            Tuple<Func<T, int>, int> stubFunction = null,
            Action<T> throwsFunction = null)
            where T : class
        {
            var mock = MockRepository.GenerateMock<T>();

            if (null != stubFunction)
                mock.Stub(x => stubFunction.Item1(x))
                    .Return(stubFunction.Item2);

            if (null != throwsFunction)
                mock.Stub(x => throwsFunction(x))
                    .Throw(new Exception());

            return mock.StubFromInstance(actual);
        }
    }
}
