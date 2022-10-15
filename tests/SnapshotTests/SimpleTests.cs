using System.Text;

namespace TupleOverloadGenerator.Tests;

[UsesVerify]
public class SimpleTests
{
    [Fact]
    public Task GeneratesEnumExtensionsCorrectly()
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

    public string Concat<T>([TupleOverload] params T[] parts) {
        ReadOnlySpan<T> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }
}
";
        return TestHelper.Verify(source);
    }
}
