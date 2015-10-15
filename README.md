# RhinoMoq.FromInstance
Extension for both [RhinoMocks](https://github.com/RhinoMocks/RhinoMocks) and [Moq](https://github.com/Moq/moq4) - Create Mocks from an Instance of an Interface

### What It Does: 

This extension solves the problem of generating a mock from an **existing** instance.  This is handy when you have are testing a class `Foo` that has a dependency on a complex object `Bar` and you want some of `Bar`'s members to be mocked and the remaining to execute against the original member.

In other words, *this saves you from intializing every member*.

### Example

Given an *Interface* and *Implementation*
```C#
public interface IFoo 
{
    string StringProp { get; }

    int IntProp { get; }
}

public class Foo : IFoo
{
    public string StringProp { get; set; }
    public int IntProp { get; set; }
}
```

##### Moq:
```C#
var mock = 
    new Mock<IFoo>()
    .SetupFromInstance(new Foo
    {
        StringProp = "Actual",
        IntProp = 42
    });

//mock a method
mock.Setup(x => x.IntProp).Returns(25);

//mock works
Assert.Equal(
    25,
    mock.Object.IntProp);

//original property works
Assert.Equal(
    "Actual",
    mock.Object.StringProp);
```

##### Rhino Mocks:
```C#
var mock = MockRepository.GenerateMock<IFoo>();

//mock a method
mock
    .Stub(x => x.IntProp)
    .Do(new Func<int>(() => 25));

//stub the rest from instance
mock
    .StubFromInstance(new Foo
    {
        StringProp = "Actual",
        IntProp = 42
    });

//mock works
Assert.AreEqual(
    25,
    mock.IntProp);

//original property works
Assert.AreEqual(
    "Actual",
    mock.StringProp);
```

### Is This Different From an AutoMocker?
Yes.  AutoMocker will create a default mock of all dependencies, but will not initialize the mocks.  But AutoMockers and `FromInstance` will play nice together.
