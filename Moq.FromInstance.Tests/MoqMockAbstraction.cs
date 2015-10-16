using System;
using RhinoMoq.FromInstance.Tests.TemplateExtensionTests;

namespace Moq.FromInstance.Tests
{
    public class MoqMockAbstraction : IMockAbstraction
    {
        public T CreateMockAndSetupFromInstance<T>(
            T actual, 
            Tuple<Func<T, int>, int> stubFunction = null, 
            Action<T> throwsFunction = null) 
            where T : class
        {
            var mock = new Mock<T>().SetupFromInstance(actual);
            
            if (null != stubFunction)
                mock.Setup(x => stubFunction.Item1(x))
                    .Returns(stubFunction.Item2);

            if (null != throwsFunction)
                mock.Setup(x => throwsFunction(x))
                    .Throws(new Exception());

            return mock.Object;
        }
    }
}
