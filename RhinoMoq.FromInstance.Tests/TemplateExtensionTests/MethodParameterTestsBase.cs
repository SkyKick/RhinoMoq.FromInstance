using System;
using RhinoMoq.FromInstance.Tests.TemplateExtensionTests.Mockables;
using Should;
using Xunit;

namespace RhinoMoq.FromInstance.Tests.TemplateExtensionTests
{
    public abstract class MethodParameterTestsBase<TMockAbstraction>
        where TMockAbstraction : IMockAbstraction, new()
    {
        private readonly IMockAbstraction mockAbstraction = new TMockAbstraction();

        [Fact]
        public void MethodsWithParametersAreMocked()
        {
            // ARRANGE
            var actual = new Foo();

            // ACT
            var mock = mockAbstraction.CreateMockAndSetupFromInstance<IFoo>(actual);
            
            // ASSERT
            mock.SumNumbers(3,6)
                .ShouldEqual(actual.SumNumbers(3,6));

            mock.SumNumbers(1,2,3,4,5)
                .ShouldEqual(actual.SumNumbers(1, 2, 3, 4, 5 ));

            mock.SumNumbers(new [] {new ComplexObject(42), new ComplexObject(5)})
                .ShouldEqual(actual.SumNumbers(new[] { new ComplexObject(42), new ComplexObject(5) }));

            Console.WriteLine("SUCCESS");
        }
    }
}