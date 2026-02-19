using System.Collections;
using CreateAndFake.Samples.Scenarios;
using CreateAndFake.ValuerTool.Hints;

namespace CreateAndFake.Tests.ValuerTool.Hints;

public sealed class StructuralEquatableCompareHintTests
    : CompareHintTestBase<StructuralEquatableCompareHint>
{
    private static readonly StructuralEquatableCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [];

    private static readonly Type[] _InvalidTypes = [typeof(IDictionary), typeof(DataHolderSample)];

    public StructuralEquatableCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
