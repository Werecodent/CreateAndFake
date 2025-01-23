using System.Collections;
using CreateAndFake.Design;
using CreateAndFake.Toolbox.ValuerTool.CompareHints;

namespace CreateAndFakeTests.Toolbox.ValuerTool.CompareHints;

public sealed class EquatableCompareHintTests : CompareHintTestBase<EquatableCompareHint>
{
    private static readonly EquatableCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(Limiter)];

    private static readonly Type[] _InvalidTypes = [typeof(IDictionary), typeof(object)];

    public EquatableCompareHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
