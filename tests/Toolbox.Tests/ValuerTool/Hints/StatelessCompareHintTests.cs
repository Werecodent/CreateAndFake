using System.Collections;
using CreateAndFake.Samples.OldSamples;
using CreateAndFake.ValuerTool.Hints;

namespace CreateAndFake.Tests.ValuerTool.Hints;

public sealed class StatelessCompareHintTests : CompareHintTestBase<StatelessCompareHint>
{
    private static readonly StatelessCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(StatelessSample)];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(object),
        typeof(string),
        typeof(IList),
        typeof(int),
    ];

    public StatelessCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    public override Task TryCompare_SupportsDifferentValidTypes()
    {
        // Stateless objects can't be different.
        return Task.CompletedTask;
    }
}
