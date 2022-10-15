using System.Runtime.CompilerServices;
using System.Text;

using FluentAssertions;

using Xunit;

namespace TupleOverloadGenerator.UsageTests;

public class AffixConcatTests {
    [Fact]
    public void TupleOneTest() {
        AffixConcat concat = new("[", ", ", "]");
        concat.Concat(ValueTuple.Create("First")).Should().BeEquivalentTo(concat.Concat("First"));
    }

    [Fact]
    public void TupleTwoTest() {
        AffixConcat concat = new("[", ", ", "]");
        concat.Concat(("First", "Second")).Should().BeEquivalentTo(concat.Concat("First", "Second"));
    }

    [Fact]
    public void TupleThreeTest() {
        AffixConcat concat = new("[", ", ", "]");
        concat.Concat(("First", "Second", "Third")).Should().BeEquivalentTo(concat.Concat("First", "Second", "Third"));
    }
}
