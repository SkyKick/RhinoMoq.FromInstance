using System;
using RhinoMoq.FromInstance.Tests.TemplateExtensionTests.Mockables;
using Should;
using Xunit;

namespace RhinoMoq.FromInstance.Tests.TemplateExtensionTests
{
    public abstract class SimpleMethodAndPropertyAccessTestsBase<TMockAbstraction>
        where TMockAbstraction : IMockAbstraction, new()
    {
        private readonly IMockAbstraction mockAbstraction = new TMockAbstraction();

        [Fact]
        public void AllOriginalMembersArePropagatedByMock()
        {
            // ARRANGE
            var actual = new Foo
            {
                StringProp = "Actual",
                IntProp = 42
            };
               
            // ACT
            var mock = mockAbstraction.CreateMockAndSetupFromInstance(actual);

            actual.TestSetProperty = 8;
            mock.TestSetProperty = 8;

            // ASSERT
            mock.StringProp
                .ShouldEqual(
                    actual.StringProp,
                    "StringProp - Property Get");

            mock.IntProp
                .ShouldEqual(
                    actual.IntProp,
                    "IntProp - Property Get");

            mock.TestSetProperty
                .ShouldEqual(
                    actual.TestSetProperty,
                    "TestSetProperty - Property Set");

            mock.GetSimpleValue()
                .ShouldEqual(
                    actual.GetSimpleValue(),
                    "GetSimpleValue() - Method Access");

            mock.GetComplexObject()
                .ShouldNotBeNull("GetComplexObject() - Method Access");

            mock.GetComplexObject().MagicValue
                .ShouldEqual(
                    actual.GetComplexObject().MagicValue,
                    "GetComplexObject() - Method Access");

            Console.WriteLine("SUCCESS");
        }
    }
}
