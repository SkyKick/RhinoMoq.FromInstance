using RhinoMoq.FromInstance.Tests.TemplateExtensionTests;

namespace RhinoMocks.FromInstance.Tests
{
    public class MethodParameterTests : MethodParameterTestsBase<RhinoMocksMockAbstraction>{}

    public class SimpleMethodAndPropertyAccessTests : SimpleMethodAndPropertyAccessTestsBase<RhinoMocksMockAbstraction> { }

    public class MocksCanStubTests : MocksCanStubTestsBase<RhinoMocksMockAbstraction> { }
}
