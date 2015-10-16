using System.Collections.Generic;
using System.Linq;

namespace RhinoMoq.FromInstance.Tests.TemplateExtensionTests.Mockables
{
    public interface IFoo
    {
        string StringProp { get; }

        int IntProp { get; }

        int GetSimpleValue();

        ComplexObject GetComplexObject();

        int TestSetProperty { get; set; }

        int SumNumbers(int a, int b);
        int SumNumbers(params int[] args);

        int SumNumbers(IEnumerable<ComplexObject> complexObjects);
    }

    public class Foo : IFoo
    {
        public string StringProp { get; set; }
        public int IntProp { get; set; }
        public int GetSimpleValue()
        {
            return 42;
        }

        public ComplexObject GetComplexObject()
        {
            return new ComplexObject(75);
        }

        public int TestSetProperty { get; set; }

        public int SumNumbers(int a, int b)
        {
            return a + b;
        }

        public int SumNumbers(params int[] args)
        {
            return args.Sum(x => x);
        }

        public int SumNumbers(IEnumerable<ComplexObject> complexObjects)
        {
            return complexObjects.Sum(x => x.MagicValue);
        }
    }
    
    public class ComplexObject
    {
        public ComplexObject(int i)
        {
            MagicValue = i;
        }

        public int MagicValue { get; set; }
    }
}
