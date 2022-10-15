//HintName: TestClass.TupleOverload.g.cs
using System;
using System.Text;

namespace TupleOverloadGenerator.Tests;

internal partial class TestClass {

    public string ConcatWithMinimum<T>([TupleOverload(Minimum = 18)] (T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T) parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }

    public string ConcatWithMinimum<T>([TupleOverload(Minimum = 18)] (T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T) parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }

    public string ConcatWithMinimum<T>([TupleOverload(Minimum = 18)] (T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T) parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }

    public string ConcatWithMinimum<T>([TupleOverload(Minimum = 18)] (T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T) parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }
    public string ConcatWithMaximum<T>([TupleOverload(Maximum = 4)] ValueTuple<T> parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }
    public string ConcatWithMaximum<T>([TupleOverload(Maximum = 4)] (T,T) parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }
    public string ConcatWithMaximum<T>([TupleOverload(Maximum = 4)] (T,T,T) parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }
    public string ConcatWithMaximum<T>([TupleOverload(Maximum = 4)] (T,T,T,T) parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }
    public string ConcatWithMinimumAndMaximum<T>([TupleOverload(Minimum = 4, Maximum = 8)] (T,T,T,T) parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }
    public string ConcatWithMinimumAndMaximum<T>([TupleOverload(Minimum = 4, Maximum = 8)] (T,T,T,T,T) parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }
    public string ConcatWithMinimumAndMaximum<T>([TupleOverload(Minimum = 4, Maximum = 8)] (T,T,T,T,T,T) parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }
    public string ConcatWithMinimumAndMaximum<T>([TupleOverload(Minimum = 4, Maximum = 8)] (T,T,T,T,T,T,T) parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }
    public string ConcatWithMinimumAndMaximum<T>([TupleOverload(Minimum = 4, Maximum = 8)] (T,T,T,T,T,T,T,T) parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }
}
