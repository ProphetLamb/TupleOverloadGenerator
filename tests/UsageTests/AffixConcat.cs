using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace TupleOverloadGenerator.UsageTests;


public partial record AffixConcat(string Prefix, string Infix, string Suffix) {
    public string Concat(ReadOnlySpan<string> parts) {
        StringBuilder sb = new();
        sb.Append(Prefix);
        var en = parts.GetEnumerator();
        if (en.MoveNext()) {
            sb.Append(en.Current);
            while (en.MoveNext()) {
                sb.Append(Infix);
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

    public string ConcatFourOrMore([TupleOverload(Minimum = 4)] params string[] parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        if (partsSpan.Length < 4) {
            ThrowLessThenFour();
        }
        return Concat(partsSpan);
    }

    [DoesNotReturn]
    private static void ThrowLessThenFour() {
        throw new ArgumentOutOfRangeException("parts", "Parts must contain four or more elements");
    }
}
