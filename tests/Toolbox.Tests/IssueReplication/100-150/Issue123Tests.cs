using System.Collections.Frozen;
using System.Collections.Immutable;

namespace CreateAndFake.Tests.IssueReplication;

public static class Issue123Tests
{
    [Theory, RandomData]
    internal static void Issue123_ImmutablesSupported(ImmutableArray<string> data)
    {
        data.Assert().Is(data.CreateDeepClone());
    }

    [Theory, RandomData]
    internal static void Issue123_FrozenDictionarySupported(FrozenDictionary<string, int> data)
    {
        data.Assert().Is(data.CreateDeepClone());
    }

    [Fact]
    internal static void Issue123_FrozenSetSupported()
    {
        FrozenSet<string> data = Tools.Randomizer.Create<FrozenSet<string>>();
        data.Assert().Is(data.CreateDeepClone());
    }
}
