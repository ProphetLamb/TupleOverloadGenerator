using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TupleOverloadGenerator;

internal  static class Helper {
    public static string AttributeName => "TupleOverloadAttribute";
    public static string AttributeTypeName => "System.TupleOverloadAttribute";

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
