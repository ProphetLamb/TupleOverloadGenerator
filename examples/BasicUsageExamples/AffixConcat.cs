using System;
using System.Text;

namespace BasicUsageExamples;

public readonly partial record struct AffixConcat(string Prefix, string Suffix) {
    public string ConcatInternal(ReadOnlySpan<string> parts) {
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

    public string Concat([TupleOverload] params string[] parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return ConcatInternal(partsSpan);
    }
}