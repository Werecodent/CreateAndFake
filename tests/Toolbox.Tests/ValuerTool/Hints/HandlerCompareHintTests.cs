using System.Collections;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class HandlerCompareHintTests : CompareHintTestBase<HandlerCompareHint>
{
    private static readonly HandlerCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [.. _TestInstance.SupportedTypes];

    private static readonly Type[] _InvalidTypes = [typeof(IDictionary), typeof(DataHolderSample)];

    public HandlerCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
