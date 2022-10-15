using System.Runtime.CompilerServices;
using System.Text;

using FluentAssertions;

using Xunit;

namespace TupleOverloadGenerator.UsageTests;

public class AffixConcatTests {
    [Fact]
    public void TupleTwoTest() {
        AffixConcat concat = new("[", "]");
        concat.Concat(("First", "Second")).Should().BeEquivalentTo(concat.Concat("First", "Second"));
    }
}

public partial record AffixConcat(string Prefix, string Suffix) {
    public string Concat(ReadOnlySpan<string> parts) {
        StringBuilder sb = new();
        sb.Append(Prefix);
        var en = parts.GetEnumerator();
        if (en.MoveNext()) {
            sb.Append(en.Current);
            while (en.MoveNext()) {
                sb.Append(", ");
                sb.Append(en.Current);
            }
        }
        sb.Append(Suffix);
        return sb.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Concat([TupleOverload] params string[] parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }
}