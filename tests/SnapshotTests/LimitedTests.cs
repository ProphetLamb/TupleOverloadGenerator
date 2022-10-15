using System.Text;

namespace TupleOverloadGenerator.Tests;

[UsesVerify]
public class LimitedTests
{
    [Fact]
    public Task MaximumAndMinimumValid()
    {
        // The source code to test
        var source = @"using System;
using System.Text;

namespace TupleOverloadGenerator.Tests;

internal partial class TestClass {
    public string Concat<T>(ReadOnlySpan<T> parts) {
        StringBuilder sb = new();
        foreach(T part in parts) {
            sb.Append(part);
        }
        return sb.ToString();
    }

    public string ConcatWithMinimum<T>([TupleOverload(Minimum = 18)] params T[] parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }
    public string ConcatWithMaximum<T>([TupleOverload(Maximum = 4)] params T[] parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }
    public string ConcatWithMinimumAndMaximum<T>([TupleOverload(Minimum = 4, Maximum = 8)] params T[] parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }
}
";
        return TestHelper.Verify(source);
    }

    [Fact]
    public Task MaximumAndMinimumInvalid()
    {
        // The source code to test
        var source = @"using System;
using System.Text;

namespace TupleOverloadGenerator.Tests;

internal partial class TestClass {
    public string Concat<T>(ReadOnlySpan<T> parts) {
        StringBuilder sb = new();
        foreach(T part in parts) {
            sb.Append(part);
        }
        return sb.ToString();
    }

    public string ConcatWithMinimum<T>([TupleOverload(Minimum = 0)] params T[] parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }
    public string ConcatWithMaximum<T>([TupleOverload(Maximum = 24)] params T[] parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }
    public string ConcatWithMinimumAndMaximum<T>([TupleOverload(Minimum = 8, Maximum = 3)] params T[] parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }
}
";
        return TestHelper.Verify(source);
    }
}
