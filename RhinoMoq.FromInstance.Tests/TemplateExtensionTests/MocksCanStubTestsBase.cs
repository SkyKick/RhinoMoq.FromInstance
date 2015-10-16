using System;
using RhinoMoq.FromInstance.Tests.TemplateExtensionTests.Mockables;
using Should;
using Xunit;

namespace RhinoMoq.FromInstance.Tests.TemplateExtensionTests
{
    public abstract class MocksCanStubTestsBase<TMockAbstraction>
        where TMockAbstraction : IMockAbstraction, new()
    {
        private readonly IMockAbstraction mockAbstraction = new TMockAbstraction();

        [Fact]
        public void MocksCanStub()
        {
            // ARRANGE
            var actual = new Foo();

            // ACT
            var mock = mockAbstraction.CreateMockAndSetupFromInstance(
                actual,
                new Tuple<Func<Foo, int>, int>(x => x.GetSimpleValue(), 100));
                
            // ASSERT
            mock.GetSimpleValue()
                .ShouldEqual(100);

            Console.WriteLine("SUCCESS");
        }

        [Fact]
        public void MocksCanThrowException()
        {
            // ARRANGE
            var actual = new Foo();

            // ACT
            var mock = mockAbstraction.CreateMockAndSetupFromInstance(
                actual,
                throwsFunction:
                    x => x.GetSimpleValue());
            
            // ASSERT
            Assert.Throws<Exception>(
                () => mock.GetSimpleValue());

            Console.WriteLine("SUCCESS");
        }
        
    }
}
