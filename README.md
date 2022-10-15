# TupleOverloadGenerator

## Supported

[Generator](https://www.nuget.org/packages/TupleOverloadGenerator) and [Types](https://www.nuget.org/packages/TupleOverloadGenerator.Types) NuGet package:
```xml
<PackageReference Include="TupleOverloadGenerator.Types" Version="1.0.1" />
<PackageReference Include="TupleOverloadGenerator" Version="1.0.1" ReferenceOutputAssembly="false" OutputItemType="Analyzer" />
```

* .NET 6.0 or greater
* .NETStandard 2.1 compatible with [System.Memory](https://www.nuget.org/packages/System.Memory) and [System.Runtime.CompilerServices.Unsafe](https://www.nuget.org/packages/System.Runtime.CompilerServices.Unsafe) packages installed.

This is experimental and uses undefined behaviour! Only tested on Linux

## Motivation

When producing a library we often wish to allow a variable number of arguments to be passed to a given function, such as string `Concat`enation.
Historically the `params` keyword followed by an array type `string[]` has been to conveniently indroduce a parameter with a variable number of arguments.
However a array introduces a few problems, the most prevalent of which is that the array is allocated on the heap.

Modern libraries should therefore allow the user to pass a `Span` instead of an array, this approach is the most performant, yet calling the function is inconvenient and still requires a heap allocation for non managed, blittable types, where `stackalloc` is not usable.
```csharp
Span<string> parts = stackalloc string[12];
parts[0] = "Hello"; [...]
AffixConcat concat = new("[", "]");
return concat.Concat(parts);
```

The solution is overloads with inline arrays. These can be represented by tuples with a single generic type parameter. This source generator generates these overloads for parameters decorated with the `[TupleOverload]` attribute.

```csharp
AffixConcat concat = new("[", ", ", "]");
return concat.Concat(("Hello", "World"));
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

The above example displays the required conditions required to use the attribute.

1. A namespace directly containing a type definition. Ommited namespace doesnt allow partial definitions, and nested types are not supported.
2. The partial type definition, e.g. `sealed partial class`, `partial record`, `partial ref struct`, ...
3. A method with a [parameter array](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/params), e.g. `params string[]`.
4. The parameter array decorated with the `[TupleOverload]` attribute.
5. The parameter **exclusively** called like with the `AsSpan` extension method. **No other member can be used!**

Please note that the example above is for demonstration purposes only! I advice using a `ref struct` and a `ValueStringBuilder` for real world applications.

## Behind the scenes

Primarly the source generator **replaces** the parameter type with a given tuple type (e.g `(string, string, string)`) for two through seven parameters. Tuples are fast for 2-7 parameters.

**Q: Tuples cannot be cast to a span can they?**
No, they cannot. At least not trivially. To obtain a span from a tuple, we have to cheat, and by cheat I mean unsafe hacks that may not work in the future.

The source generator adds the following extension methods to the value types types with 2-7 parameters:

```csharp
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
[...]
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static ref T GetPinnableReference<T>(in this ValueTuple<T, T> tuple) {
    return ref Unsafe.AsRef(in tuple).Item1;
}
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static ReadOnlySpan<T> AsSpan<T>(in this ValueTuple<T, T> tuple) {
    return MemoryMarshal.CreateReadOnlySpan(ref tuple.GetPinnableReference(), 2);
}
```

### GetPinnableReference
GetPinnableReference returns a reference to the first element in the tuple, treating it as a linline array.
This is unsafe, because RYU may reorder the the items, so that the following layout applies:

```js
[Item2][padding][Item1][padding][Item3][padding]
```

If the structure has padding, or is reordered this will not work! Therefore its best used with pointer sized values, such as `nint`, `object`, `Func<>`, etc..

### AsSpan
As span creates a span from the reference to the first element in the tuple with length equal to the number of elements in the tuple.

The primary issue here is that [MemoryMarshal.CreateReadOnlySpan](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.memorymarshal.createreadonlyspan?view=net-6.0) is not speified to work with a tuple! At some point Microsoft may deicide that this should throw an exception instead of succeeding. We are working with undefined behaviour here!

Other then that the `in` keyword for the parameter too can be a problem. It specifies that the readonly-**reference** to the struct is passed instead of the struct itself. In and of itself this is not a problem, but the memory analyzer will complain when returing the span to a different context.

All in all I have tested this with `3.1.423`, `6.0.401` and `7.0.0-rc.2.22472.3` on **Linux**. This is untested on MacOs and Windows.
