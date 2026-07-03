using System.Collections.Frozen;
using System.Collections.Specialized;
using CreateAndFake.Design.Reiteration;
using CreateAndFake.MutatorTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.MutatorTool.Hints;

public sealed class DictionaryMutateHintTests : MutateHintTestBase<DictionaryMutateHint>
{
    [Fact]
    public void Modify_ImmutableSamplesFalse()
    {
        RunModifyTest<FrozenDictionary<int, string>>(false, 0);
        RunModifyTest<FrozenDictionary<int, int>>(false);
    }

    [Fact]
    public void Modify_MutableSamplesTrue()
    {
        RunModifyTest<Dictionary<string, DataSample>>(true, 0);
        RunModifyTest<Dictionary<string, DataSample>>(true);
        RunModifyTest<Dictionary<int, int>>(true, 0);
        RunModifyTest<Dictionary<int, int>>(true);
        RunModifyTest<ListDictionary>(true, 0);
        RunModifyTest<ListDictionary>(true);
        RunModifyTest<HybridDictionary>(true, 0);
        RunModifyTest<HybridDictionary>(true);
    }

    [Theory, RandomData]
    public void Modify_UsesInternalType(int key, DataSample value)
    {
        ListDictionary data = new() { { key, value } };
        ListDictionary original = data.Tools().Copy();

        Limiter.Score.StallUntil(
            "Until added to.",
            () => TestInstance.TryToModify(data, CreateChainer()),
            () => data.Count > original.Count,
            TestContext.Current.CancellationToken
        );

        data.Assert().IsNot(original);
        data.Keys.OfType<int>().Assert().HasCount(2);
        data.Values.OfType<DataSample>().Assert().HasCount(2);
    }
}
