using RhinoMoq.FromInstance.Tests.TemplateExtensionTests;

namespace Moq.FromInstance.Tests
{
    public class MethodParameterTests : MethodParameterTestsBase<MoqMockAbstraction>{}

    public class SimpleMethodAndPropertyAccessTests : SimpleMethodAndPropertyAccessTestsBase<MoqMockAbstraction> { }

    public class MocksCanStubTests : MocksCanStubTestsBase<MoqMockAbstraction> { }
}
