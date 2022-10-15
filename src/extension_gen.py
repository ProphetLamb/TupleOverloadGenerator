#!/usr/bin/env python
import itertools

snippet="""
        /// <inheritdoc cref="System.Span{T}.GetPinnableReference()" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetPinnableReference<T>(in this ($1) tuple) {
            return ref Unsafe.AsRef(in tuple).Item1;
        }

        /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(ref this ($1) tuple) {
            return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), $2);
        }

        /// <inheritdoc cref="System.MemoryExtensions.AsSpan{T}(T[])" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> AsRoSpan<T>(in this ($1) tuple) {
            return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), $2);
        }
"""

all_snippets=[snippet.replace("$1", ", ".join(itertools.repeat("T", i))).replace("$2", str(i)) for i in range(2, 21)]
text="\n\n".join(all_snippets)
print(text)
