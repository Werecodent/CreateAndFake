using System.Collections;
using System.Collections.Specialized;
using CreateAndFake.Toolbox.ValuerTool.CompareHints;

namespace CreateAndFakeTests.Toolbox.ValuerTool.CompareHints;

public sealed class StringDictionaryCompareHintTests : CompareHintTestBase<StringDictionaryCompareHint>
{
    private static readonly StringDictionaryCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(StringDictionary)];

    private static readonly Type[] _InvalidTypes = [typeof(IDictionary), typeof(object)];

    public StringDictionaryCompareHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
