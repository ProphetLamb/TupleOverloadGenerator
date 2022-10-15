using System;
using System.Text;

namespace BasicUsageExamples; 

internal sealed partial class TestClass {
    public string Concat<T>(ReadOnlySpan<T> parts) {
        StringBuilder sb = new();
        foreach(T part in parts) {
            sb.Append(part);
        }
        return sb.ToString();
    }

    public string Concat<T>([TupleOverload] params T[] parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }
}