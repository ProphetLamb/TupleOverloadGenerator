using System.Text;

namespace TupleOverloadGenerator.Tests;

[UsesVerify]
public class NestedGenericTests
{
    [Fact]
    public Task GeneratesEnumExtensionsCorrectly()
    {
        // The source code to test
        var source = @"using System;
using System.Text;

namespace TupleOverloadGenerator.Tests;

internal partial class TestClass {
    public string Concat<T>(ReadOnlySpan<ReadOnlyMemory<T>> parts) {
        StringBuilder sb = new();
        foreach(T part in parts) {
            sb.Append(part);
        }
        return sb.ToString();
    }

    public string Concat<T>([TupleOverload] params ReadOnlyMemory<T>[] parts) {
        ReadOnlySpan<ReadOnlyMemory<T>> partsSpan = parts.AsSpan();
        return Concat<T>(partsSpan);
    }
}
";
        return TestHelper.Verify(source);
    }
}
