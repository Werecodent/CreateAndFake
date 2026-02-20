using System.Collections;
using CreateAndFake.Design.Reiteration;
using CreateAndFake.Samples.Scenarios;
using CreateAndFake.ValuerTool.Hints;

namespace CreateAndFake.Tests.ValuerTool.Hints;

public sealed class EquatableCompareHintTests : CompareHintTestBase<EquatableCompareHint>
{
    private static readonly EquatableCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(Limiter)];

    private static readonly Type[] _InvalidTypes = [typeof(IDictionary), typeof(DataHolderSample)];

    public EquatableCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
