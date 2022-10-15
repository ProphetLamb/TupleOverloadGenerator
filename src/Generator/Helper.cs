using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TupleOverloadGenerator;

public static class Helper {
    public static string AttributeName => "TupleOverloadAttribute";
    public static string AttributeTypeName => "System.TupleOverloadAttribute";
    public static string AttributeSource => @"namespace System {
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public class TupleOverloadAttribute: Attribute {

    }
}
";

    public static string ExtensionName => "TupleExtensions";
    public static string ExtensionSource => @"using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System {

    public static class TupleExtensions {
        // Yea this is funky
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetPinnableReference<T>(in this ValueTuple<T, T> tuple) {
            return ref Unsafe.AsRef(in tuple).Item1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetPinnableReference<T>(in this ValueTuple<T, T, T> tuple) {
            return ref Unsafe.AsRef(in tuple).Item1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetPinnableReference<T>(in this ValueTuple<T, T, T, T> tuple) {
            return ref Unsafe.AsRef(in tuple).Item1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetPinnableReference<T>(in this ValueTuple<T, T, T, T, T> tuple) {
            return ref Unsafe.AsRef(in tuple).Item1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetPinnableReference<T>(in this ValueTuple<T, T, T, T, T, T> tuple) {
            return ref Unsafe.AsRef(in tuple).Item1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetPinnableReference<T>(in this ValueTuple<T, T, T, T, T, T, T> tuple) {
            return ref Unsafe.AsRef(in tuple).Item1;
        }

        // Allows accessing the contents of a ValueTuple like with the ITuple indexer, with no casting or copying involved
        public static ReadOnlySpan<T> RefSpan<T>(in this ValueTuple<T, T> tuple) {
            return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 2);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> RefSpan<T>(in this ValueTuple<T, T, T> tuple) {
            return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 3);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> RefSpan<T>(in this ValueTuple<T, T, T, T> tuple) {
            return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 4);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> RefSpan<T>(in this ValueTuple<T, T, T, T, T> tuple) {
            return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 5);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> RefSpan<T>(in this ValueTuple<T, T, T, T, T, T> tuple) {
            return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 6);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> RefSpan<T>(in this ValueTuple<T, T, T, T, T, T, T> tuple) {
            return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 7);
        }

        // Allows modifying the ValueTuple. Note that this gets dirty memory when not inlined by the RYU, only tested with net6 and net7rc1
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this ValueTuple<T, T> tuple) {
            return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 2);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this ValueTuple<T, T, T> tuple) {
            return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 3);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this ValueTuple<T, T, T, T> tuple) {
            return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 4);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this ValueTuple<T, T, T, T, T> tuple) {
            return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 5);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this ValueTuple<T, T, T, T, T, T> tuple) {
            return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 6);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this ValueTuple<T, T, T, T, T, T, T> tuple) {
            return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 7);
        }
    }
}
";

    public static IEnumerable<AttributeSyntax> Attributes(this SyntaxList<AttributeListSyntax> lst)
    {
        foreach (AttributeListSyntax attributeListSyntax in lst)
        {
            foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
            {
                yield return attributeSyntax;
            }
        }
    }

    public static T? ParentOf<T>(this SyntaxNode? node)
    {
        while (node is not null)
        {
            if (node is T res)
            {
                return res;
            }

            node = node.Parent;
        }

        return default;
    }

    public static IEnumerable<U> FilterMap<T, U>(this IEnumerable<T> nodes, Func<T, U?> f)
        where U : class
    {
        foreach (T element in nodes)
        {
            if (f(element) is U res)
            {
                yield return res;
            }
        }
    }

    public static IEnumerable<U> FilterMap<T, U>(this IEnumerable<T> nodes, Func<T, Nullable<U>> f)
        where U : struct
    {
        foreach (T element in nodes)
        {
            var res = f(element);
            if (res.HasValue)
            {
                yield return res.Value;
            }
        }
    }

    public static Nullable<T> Nullable<T>(this T value)
        where T : struct
    {
        return value;
    }
}
