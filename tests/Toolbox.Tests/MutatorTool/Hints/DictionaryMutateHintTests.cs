using System.Collections.Frozen;
using System.Collections.Specialized;
using Werecodent.CreateAndFake.Design.Reiteration;
using Werecodent.CreateAndFake.MutatorTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.MutatorTool.Hints;

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
        ListDictionary data = new() { { key.Tools().Variant(), null }, { key, value } };
        ListDictionary original = data.Tools().Copy();

        Limiter.Score.StallUntil(
            "Until added to.",
            () => TestInstance.TryToModify(data, CreateChainer()),
            () => data.Count > original.Count,
            TestContext.Current.CancellationToken
        );

        data.Assert()
            .IsNot(original)
            .Also(data.Keys.OfType<int>())
            .HasCount(3)
            .Also(data.Values.OfType<DataSample>())
            .HasCountMoreThan(1);
    }
}
