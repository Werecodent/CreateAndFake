using CreateAndFake.AsserterTool;

namespace CreateAndFake.Tests.AsserterTool;

public static class AsserterStringTests
{
    [Theory, RandomData]
    internal static void Contains_UsingSubstring(ICollection<string> sample)
    {
        string.Concat(sample).Assert().Contains(Tools.Gen.NextItem(sample));
    }

    [Theory, RandomData]
    internal static void Contains_RandomOther(string sample)
    {
        sample
            .Assert(d => d.Assert().Contains(Tools.Mutator.Variant(sample)))
            .Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void ContainsNot_RandomOther(string sample)
    {
        sample.Assert().ContainsNot(Tools.Mutator.Variant(sample));
    }

    [Theory, RandomData]
    internal static void ContainsNot_UsingSubstring(ICollection<string> sample)
    {
        string.Concat(sample)
            .Assert(d => d.Assert().ContainsNot(Tools.Gen.NextItem(sample)))
            .Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void StartsWith_UsingFirstString(ICollection<string> sample)
    {
        string.Concat(sample).Assert().StartsWith(sample.First());
    }

    [Theory, RandomData]
    internal static void StartsWith_UsingNonFirstString([Size(3)] ICollection<string> sample)
    {
        string.Concat(sample)
            .Assert(d => d.Assert().StartsWith(Tools.Gen.NextItem(sample.Skip(1))))
            .Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void StartsNotWith_UsingNonFirstString([Size(3)] ICollection<string> sample)
    {
        string.Concat(sample).Assert().StartsNotWith(Tools.Gen.NextItem(sample.Skip(1)));
    }

    [Theory, RandomData]
    internal static void StartsNotWith_UsingFirstString(ICollection<string> sample)
    {
        string.Concat(sample)
            .Assert(d => d.Assert().StartsNotWith(sample.First()))
            .Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void EndsWith_UsingLastString(ICollection<string> sample)
    {
        string.Concat(sample).Assert().EndsWith(sample.Last());
    }

    [Theory, RandomData]
    internal static void EndsWith_UsingNonLstString([Size(3)] ICollection<string> sample)
    {
        string.Concat(sample)
            .Assert(d => d.Assert().EndsWith(Tools.Gen.NextItem(sample.Reverse().Skip(1))))
            .Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void EndsNotWith_UsingNonLastString([Size(3)] ICollection<string> sample)
    {
        string.Concat(sample).Assert().EndsNotWith(Tools.Gen.NextItem(sample.Reverse().Skip(1)));
    }

    [Theory, RandomData]
    internal static void EndsNotWith_UsingLstString(ICollection<string> sample)
    {
        string.Concat(sample)
            .Assert(d => d.Assert().EndsNotWith(sample.Last()))
            .Throws<AssertException>();
    }
}
