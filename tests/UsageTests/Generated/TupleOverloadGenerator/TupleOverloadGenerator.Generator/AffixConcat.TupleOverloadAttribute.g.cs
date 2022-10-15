using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace TupleOverloadGenerator.UsageTests;


public partial record AffixConcat{

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Concat([TupleOverload] ValueTuple<String> parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Concat([TupleOverload] (String,String) parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Concat([TupleOverload] (String,String,String) parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Concat([TupleOverload] (String,String,String,String) parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Concat([TupleOverload] (String,String,String,String,String) parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Concat([TupleOverload] (String,String,String,String,String,String) parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Concat([TupleOverload] (String,String,String,String,String,String,String) parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Concat([TupleOverload] (String,String,String,String,String,String,String,String) parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Concat([TupleOverload] (String,String,String,String,String,String,String,String,String) parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Concat([TupleOverload] (String,String,String,String,String,String,String,String,String,String) parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Concat([TupleOverload] (String,String,String,String,String,String,String,String,String,String,String) parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Concat([TupleOverload] (String,String,String,String,String,String,String,String,String,String,String,String) parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Concat([TupleOverload] (String,String,String,String,String,String,String,String,String,String,String,String,String) parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Concat([TupleOverload] (String,String,String,String,String,String,String,String,String,String,String,String,String,String) parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Concat([TupleOverload] (String,String,String,String,String,String,String,String,String,String,String,String,String,String,String) parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Concat([TupleOverload] (String,String,String,String,String,String,String,String,String,String,String,String,String,String,String,String) parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Concat([TupleOverload] (String,String,String,String,String,String,String,String,String,String,String,String,String,String,String,String,String) parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Concat([TupleOverload] (String,String,String,String,String,String,String,String,String,String,String,String,String,String,String,String,String,String) parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Concat([TupleOverload] (String,String,String,String,String,String,String,String,String,String,String,String,String,String,String,String,String,String,String) parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Concat([TupleOverload] (String,String,String,String,String,String,String,String,String,String,String,String,String,String,String,String,String,String,String,String) parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Concat([TupleOverload] (String,String,String,String,String,String,String,String,String,String,String,String,String,String,String,String,String,String,String,String,String) parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }
}
