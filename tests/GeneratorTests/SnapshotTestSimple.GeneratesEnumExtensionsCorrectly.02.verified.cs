//HintName: TestClass.TupleOverloadAttribute.g.cs
using System;
using System.Text;

internal partial class TestClass {

    public string Concat<T>([TupleOverload] (T,T) parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }

    public string Concat<T>([TupleOverload] (T,T,T) parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }

    public string Concat<T>([TupleOverload] (T,T,T,T) parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }

    public string Concat<T>([TupleOverload] (T,T,T,T,T) parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }

    public string Concat<T>([TupleOverload] (T,T,T,T,T,T) parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }

    public string Concat<T>([TupleOverload] (T,T,T,T,T,T,T) parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }
}
