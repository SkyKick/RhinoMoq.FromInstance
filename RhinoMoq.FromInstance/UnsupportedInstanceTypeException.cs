using System;

namespace RhinoMoq.FromInstance
{
    /// <summary>
    /// Currently only interfaces are supported by <see cref="FromInstanceMockingEngine.MockFromInstance{T}"/>
    /// </summary>
    public class UnsupportedInstanceTypeException : Exception
    {
        public UnsupportedInstanceTypeException(Type t) :
            base($"Type {t.FullName} is not supported.")
        { }
    }
}