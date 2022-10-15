using System;
// ReSharper disable CheckNamespace

namespace System;

/// <summary>Decorates a params array parameter, `[TupleOverload] params string[] parts`.
/// Generates overloads for the parameter with tuples consisting of 2 through 7 members.
/// The parameter `parts` must only be used to obtain a span, nothing more! The span can then be passed to another function.</summary>
[AttributeUsage(AttributeTargets.Parameter)]
public class TupleOverloadAttribute: Attribute {
    private int _minimum = 1;
    private int _maximum = 21;

    /// <summary>The minimum number of elements in the tuple. Only integer literals allowed! Minimum value 1, limited to incl. 21</summary>
    public int Minimum {
        get => _minimum;
        set {
            if (value < 1 || value > 21) {
                throw new ArgumentOutOfRangeException(nameof(value), "Value must be between one and twenty-one");
            }
            _minimum = value;
        }
    }
    /// <summary>The maximum number of elements in the tuple. Only integer literals allowed! Minimum value 1, limited to incl. 21</summary>
    public int Maximum {
        get => _maximum;
        set {
            if (value < 1 || value > 21) {
                throw new ArgumentOutOfRangeException(nameof(value), "Value must be between one and twenty-one");
            }
            _maximum = value;
        }
    }
}
