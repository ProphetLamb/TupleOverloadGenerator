//HintName: TupleExtensions.g.cs
using System.Runtime.CompilerServices;
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
