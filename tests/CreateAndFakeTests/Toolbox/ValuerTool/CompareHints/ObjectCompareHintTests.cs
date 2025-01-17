using System.Reflection;
using CreateAndFake.Toolbox.ValuerTool;
using CreateAndFake.Toolbox.ValuerTool.CompareHints;
using CreateAndFakeTests.TestBases;
using CreateAndFakeTests.TestSamples;

namespace CreateAndFakeTests.Toolbox.ValuerTool.CompareHints;

public sealed class ObjectCompareHintTests : CompareHintTestBase<ObjectCompareHint>
{
    private static readonly ObjectCompareHint _TestInstance = new(BindingFlags.Public | BindingFlags.Instance);

    private static readonly Type[] _ValidTypes = [typeof(object), typeof(DataHolderSample), typeof(FieldSample)];

    private static readonly Type[] _InvalidTypes = Type.EmptyTypes;

    public ObjectCompareHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal void Compare_DifferentObjectsDifferences(string value1, string value2)
    {
        var expected = new { Value = value1 };
        var actual = new { Value = value2 };

        DifferenceHintResult result = TestInstance
            .TryCompare(expected, actual, CreateChainer());

        result.HasData.Assert().Is(true);
        result.Data.Assert().IsNotEmpty();
    }
}
