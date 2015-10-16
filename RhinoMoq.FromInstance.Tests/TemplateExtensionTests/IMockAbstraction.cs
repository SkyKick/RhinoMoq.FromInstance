using System;

namespace RhinoMoq.FromInstance.Tests.TemplateExtensionTests
{
    public interface IMockAbstraction
    {
        T CreateMockAndSetupFromInstance<T>(
            T actual, 
            Tuple<Func<T, int>, int> stubFunction = null,
            Action<T> throwsFunction = null)
            where T : class;
    }
}
