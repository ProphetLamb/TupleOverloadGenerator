using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
// ReSharper disable CheckNamespace

namespace System;

/// <summary>
/// Extensions allowing Span access to a ValueTuple
/// </summary>
public static class TupleSpanExtensions {
    /// <inheritdoc cref="System.Span{T}.GetPinnableReference()" />
    public static unsafe ref T GetPinnableReference<T>(this T[] array) {
        if (array.Length > 0) {
            return ref array[0];
        }

        return ref Unsafe.AsRef<T>((void*)default(IntPtr));
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsRoSpan<T>(this T[] array) {
        return array.AsSpan();
    }

    //
    // ValueTuple of one element as `(T)` is invalid syntax. Explicit type is required.
    //

    /// <inheritdoc cref="System.Span{T}.GetPinnableReference()" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetPinnableReference<T>(in this ValueTuple<T> tuple) {
        return ref Unsafe.AsRef(in tuple).Item1;
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(ref this ValueTuple<T> tuple) {
        return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 1);
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsRoSpan<T>(in this ValueTuple<T> tuple) {
        return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 1);
    }

    //
    // generated by 'src/extension_gen.py | clip'
    //


    /// <inheritdoc cref="System.Span{T}.GetPinnableReference()" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetPinnableReference<T>(in this (T, T) tuple) {
        return ref Unsafe.AsRef(in tuple).Item1;
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(in this (T, T) tuple) {
        return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 2);
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsRoSpan<T>(in this (T, T) tuple) {
        return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 2);
    }


    /// <inheritdoc cref="System.Span{T}.GetPinnableReference()" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetPinnableReference<T>(in this (T, T, T) tuple) {
        return ref Unsafe.AsRef(in tuple).Item1;
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(in this (T, T, T) tuple) {
        return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 3);
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsRoSpan<T>(in this (T, T, T) tuple) {
        return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 3);
    }


    /// <inheritdoc cref="System.Span{T}.GetPinnableReference()" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetPinnableReference<T>(in this (T, T, T, T) tuple) {
        return ref Unsafe.AsRef(in tuple).Item1;
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(in this (T, T, T, T) tuple) {
        return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 4);
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsRoSpan<T>(in this (T, T, T, T) tuple) {
        return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 4);
    }


    /// <inheritdoc cref="System.Span{T}.GetPinnableReference()" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetPinnableReference<T>(in this (T, T, T, T, T) tuple) {
        return ref Unsafe.AsRef(in tuple).Item1;
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(in this (T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 5);
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsRoSpan<T>(in this (T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 5);
    }


    /// <inheritdoc cref="System.Span{T}.GetPinnableReference()" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetPinnableReference<T>(in this (T, T, T, T, T, T) tuple) {
        return ref Unsafe.AsRef(in tuple).Item1;
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(in this (T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 6);
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsRoSpan<T>(in this (T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 6);
    }


    /// <inheritdoc cref="System.Span{T}.GetPinnableReference()" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetPinnableReference<T>(in this (T, T, T, T, T, T, T) tuple) {
        return ref Unsafe.AsRef(in tuple).Item1;
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(in this (T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 7);
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsRoSpan<T>(in this (T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 7);
    }


    /// <inheritdoc cref="System.Span{T}.GetPinnableReference()" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetPinnableReference<T>(in this (T, T, T, T, T, T, T, T) tuple) {
        return ref Unsafe.AsRef(in tuple).Item1;
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(in this (T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 8);
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsRoSpan<T>(in this (T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 8);
    }


    /// <inheritdoc cref="System.Span{T}.GetPinnableReference()" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetPinnableReference<T>(in this (T, T, T, T, T, T, T, T, T) tuple) {
        return ref Unsafe.AsRef(in tuple).Item1;
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(in this (T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 9);
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsRoSpan<T>(in this (T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 9);
    }


    /// <inheritdoc cref="System.Span{T}.GetPinnableReference()" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetPinnableReference<T>(in this (T, T, T, T, T, T, T, T, T, T) tuple) {
        return ref Unsafe.AsRef(in tuple).Item1;
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(in this (T, T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 10);
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsRoSpan<T>(in this (T, T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 10);
    }


    /// <inheritdoc cref="System.Span{T}.GetPinnableReference()" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetPinnableReference<T>(in this (T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return ref Unsafe.AsRef(in tuple).Item1;
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(in this (T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 11);
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsRoSpan<T>(in this (T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 11);
    }


    /// <inheritdoc cref="System.Span{T}.GetPinnableReference()" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetPinnableReference<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return ref Unsafe.AsRef(in tuple).Item1;
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 12);
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsRoSpan<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 12);
    }


    /// <inheritdoc cref="System.Span{T}.GetPinnableReference()" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetPinnableReference<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return ref Unsafe.AsRef(in tuple).Item1;
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 13);
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsRoSpan<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 13);
    }


    /// <inheritdoc cref="System.Span{T}.GetPinnableReference()" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetPinnableReference<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return ref Unsafe.AsRef(in tuple).Item1;
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 14);
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsRoSpan<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 14);
    }


    /// <inheritdoc cref="System.Span{T}.GetPinnableReference()" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetPinnableReference<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return ref Unsafe.AsRef(in tuple).Item1;
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 15);
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsRoSpan<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 15);
    }


    /// <inheritdoc cref="System.Span{T}.GetPinnableReference()" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetPinnableReference<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return ref Unsafe.AsRef(in tuple).Item1;
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 16);
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsRoSpan<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 16);
    }


    /// <inheritdoc cref="System.Span{T}.GetPinnableReference()" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetPinnableReference<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return ref Unsafe.AsRef(in tuple).Item1;
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 17);
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsRoSpan<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 17);
    }


    /// <inheritdoc cref="System.Span{T}.GetPinnableReference()" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetPinnableReference<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return ref Unsafe.AsRef(in tuple).Item1;
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 18);
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsRoSpan<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 18);
    }


    /// <inheritdoc cref="System.Span{T}.GetPinnableReference()" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetPinnableReference<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return ref Unsafe.AsRef(in tuple).Item1;
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 19);
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsRoSpan<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 19);
    }


    /// <inheritdoc cref="System.Span{T}.GetPinnableReference()" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetPinnableReference<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return ref Unsafe.AsRef(in tuple).Item1;
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 20);
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsRoSpan<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 20);
    }


    /// <inheritdoc cref="System.Span{T}.GetPinnableReference()" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetPinnableReference<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return ref Unsafe.AsRef(in tuple).Item1;
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 21);
    }

    /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsRoSpan<T>(in this (T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T) tuple) {
        return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 21);
    }
}
