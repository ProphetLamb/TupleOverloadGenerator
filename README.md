
<p align="center">
	<a href="https://www.nuget.org/packages/TupleOverloadGenerator"><img src="https://img.shields.io/nuget/dt/TupleOverloadGenerator?label=Generator&style=for-the-badge" /></a>
	<a href="https://www.nuget.org/packages/TupleOverloadGenerator.Types"><img src="https://img.shields.io/nuget/dt/TupleOverloadGenerator.Types?label=Generator.Types&style=for-the-badge" /></a>
	<br/>
	<img src="img/banner.png" alt="Logo" width="305" height="125">
</p>
<h1 align="center">TupleOverloadGenerator</h1>
<p align="center">Overload <code>params</code> array parameter with tuples avoiding heap allocations.</p>

## Table of Contents

- [Table of Contents](#table-of-contents)
- [Supported](#supported)
- [Motivation](#motivation)
	- [Stackalloc](#stackalloc)
	- [Pool](#pool)
	- [Tuple](#tuple)
- [Example](#example)
	- [Generated](#generated)
- [Behind the scenes](#behind-the-scenes)
	- [Tuple as Span](#tuple-as-span)
	- [GetPinnableReference](#getpinnablereference)
	- [AsSpan & AsRoSpan](#asspan--asrospan)

## Supported

[Generator](https://www.nuget.org/packages/TupleOverloadGenerator) and [Types](https://www.nuget.org/packages/TupleOverloadGenerator.Types) NuGet package:
```xml
<PackageReference Include="TupleOverloadGenerator.Types" Version="1.0.2" />
<PackageReference Include="TupleOverloadGenerator" Version="1.0.2" ReferenceOutputAssembly="false" OutputItemType="Analyzer" />
```

* .NET Framework 4.8 or greater
* .NET 6.0 or greater
* .NETStandard 2.1 compatible. Depends on [System.Memory](https://www.nuget.org/packages/System.Memory).

## Motivation

When producing a library we often wish to allow a variable number of arguments to be passed to a given function, such as string `Concat`enation.
Historically the `params` keyword followed by an array type (e.g. `params string[]`) has been to conveniently introduce a parameter with a variable number of arguments.
However an array introduces a few problems, the most prevalent of which is that the array is allocated on the heap.

### Stackalloc

Modern libraries should therefore allow the user to pass a `Span` instead of an array, this approach is the most performant, yet calling the function is inconvenient and still requires a heap allocation for managed, and non-blittable types, where `stackalloc` is not usable.

| **DON'T** |
| --------- |
```csharp
Span<string> parts = stackalloc string[12];
parts[0] = "Hello";
parts[1] = [...]
AffixConcat concat = new("[", "]");
return concat.Concat(parts);
```

### Pool

Alternatively an `ArrayPool` can be used, in the best case reducing the number of allocations from `n` to `1` for any given size, where `n` is the number of calls to the function. We still have allocations, and the syntax becomes even more unwieldy.

| **DON'T** |
| --------- |
```csharp
var poolArray = ArrayPool<string>.Shared.Rent(12);
Span<string> parts = poolArray.AsSpan(0, 12);
parts[0] = "Hello";
parts[1] = [...]
AffixConcat concat = new("[", "]");
var result = concat.Concat(parts);
ArrayPool<string>.Shared.Return(poolArray);
```

### Tuple

The solution is overloads with inline arrays. These can be represented by tuples with a single generic type parameter used for different arities. `TupleOverloadGenerator` generates these overloads for parameter array parameters decorated with the `[TupleOverload]` attribute.
The avoids any heap allocation at all, because we can assign any type `T` to a tuple item. `struct ValueTuple<T, ...>` is the underlaying type of `(T, ...)`. By default the struct lives on the stack.

| **DO** |
| ------ |
```csharp
AffixConcat concat = new("[", ", ", "]");
return concat.Concat(("Hello", [...]));
```

## Example


```csharp
using System;
using System.Text;

namespace BasicUsageExamples;

public partial readonly record struct AffixConcat(string Prefix, string Infix, string Suffix) {
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

    public string Concat([TupleOverload] params string[] parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }
}
```

The above example displays the conditions required to use the sourcegenerator.

1. A namespace directly containing a type definition. Omitted namespace doesnt allow partial definitions, and nested types are not supported.
2. The partial type definition, e.g. `sealed partial class`, `partial record`, `partial ref struct`, ...
3. A method with a [parameter array](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/params), e.g. `params string[]`.
4. The parameter array decorated with the `[TupleOverload]` attribute.
5. The parameter **exclusively** called with [allowed methods](#behind-the-scenes). **No other member can be used!**

Please note that the example above is for demonstration purposes only! I advise using a `ref struct` and a `ValueStringBuilder` for real world applications.

The following file is generated for the example above.

### Generated

```csharp
using System;
using System.Text;

namespace BasicUsageExamples;

public partial readonly record struct AffixConcat {
    public string Concat([TupleOverload] ValueTuple<string> parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }
    public string Concat([TupleOverload] (string, string) parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }
    [...]
    public string Concat([TupleOverload] (string, [...through 21]) parts) {
        ReadOnlySpan<string> partsSpan = parts.AsSpan();
        return Concat(partsSpan);
    }
}
```

The optional parameters `TupleOverload(Minimum=1, Maximum=21)` determine which overloads are generated.

## Behind the scenes

`TupleOverloadGenerator.Types` adds three methods to tuple, which are ensured for arrays aswell, so that they can be used interchangeably. **If any members on the `params` array are called, except these methods, the generator will fail!**

- `AsSpan(): Span<T>` - Returns the span over the tuple/array
- `AsRoSpan(): ReadOnlySpan<T>` - Returns the span over the tuple/array
- `GetPinnableReference(): ref T` - Returns the pinnable reference to the first element in the tuple/array.

The sourcegenerator `TupleOverloadGenerator` primarly **replaces** the params parameter type with a given tuple type (e.g. `params string[]` -> `(string, string, string)`).

### Tuple as Span

**Tuples cannot be cast to a span can they?**
No, they cannot. At least not trivially. To obtain a span from a tuple, we have to cheat, and by cheat I mean unsafe hacks that may not work in the future.

The source generator adds the following extension methods to the value types with 1-21 parameters:

```csharp
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[...]

[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static ref T GetPinnableReference<T>(in this (T, T) tuple) {
    return ref Unsafe.AsRef(in tuple).Item1;
}
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static Span<T> AsSpan<T>(in this (T, T) tuple) {
    return MemoryMarshal.CreateSpan(ref tuple.GetPinnableReference(), 2);
}
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static ReadOnlySpan<T> AsRoSpan<T>(in this (T, T) tuple) {
    return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 2);
}

[...]
```

### GetPinnableReference
GetPinnableReference returns a reference to the first element in the tuple, treating it as a inline array.
This is unsafe, because RYU may reorder the items, so that the following layout applies:

```js
[Item2][padding][Item1][padding][Item3][padding]
```

If the structure has padding inconsistent with the array allocation padding (which is unlikely, but again undefined), or the structure is reordered this will not work! The runtime the tuntime should not change the ordering, because the element size is equal.

Note that for 9 arguments the following type is created `ValueTuple<T,T,T,T,T,T,T,ValueTuple<T,T>>` here the size of elements is not equal, reordering may occur.

### AsSpan & AsRoSpan
`AsSpan` creates a span from the reference to the first element in the tuple with length equal to the number of elements in the tuple.

The primary issue here is that [MemoryMarshal.CreateSpan](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.memorymarshal.createspan) is not specified to work with a tuple! At some point Microsoft may deicide that this should throw an exception instead of succeeding. We are working with undefined behaviour here!

Other then that the `in` keyword for the parameter too can be a problem. It specifies that the readonly-**reference** to the struct is passed instead of the struct itself. In and of itself this is not a problem, but the memory analyzer will complain when returning the span to a different context.

All in all, I have tested this with `3.1.423`, `6.0.401` and `7.0.0-rc.2.22472.3` on Linux and with .NET Framework `4.8.1` on Windows. The funcionality is untested on macOS.
