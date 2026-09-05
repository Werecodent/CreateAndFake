using System.Collections;
using Werecodent.CreateAndFake.Design.Reiteration;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class EquatableCompareHintTests : CompareHintTestBase<EquatableCompareHint>
{
    private static readonly EquatableCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(Limiter)];

    private static readonly Type[] _InvalidTypes = [typeof(IDictionary), typeof(DataHolderSample)];

    public EquatableCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
