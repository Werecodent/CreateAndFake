using System.Collections;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class DictionaryCompareHintTests : CompareHintTestBase<DictionaryCompareHint>
{
    private static readonly DictionaryCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(IDictionary),
        typeof(Dictionary<string, int>),
    ];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(DataHolderSample),
        typeof(string),
        typeof(IList),
        typeof(int),
    ];

    public DictionaryCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal void TryToCompare_SameKeyDifferentValuesWorks(Dictionary<string, int> data)
    {
        Dictionary<string, int> dupe = data.Tools().Copy();
        string key = data.First().Key;
        dupe[key] = data[key].Tools().Variant();

        TestInstance
            .TryToCompare(data, dupe, CreateChainer())
            .Data.ToArray()
            .Assert()
            .IsNotEmpty(
                "Hint didn't find differences with a modified key on '"
                    + GenericConverter.ExpandName(data)
                    + "'."
            );
    }
}
