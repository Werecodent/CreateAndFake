using CreateAndFake.Samples.Scenarios;
using CreateAndFake.ValuerTool.Engine;
using CreateAndFake.ValuerTool.Hints;

namespace CreateAndFake.Tests.ValuerTool.Hints;

public sealed class PublicObjectCompareHintTests : CompareHintTestBase<PublicObjectCompareHint>
{
    private static readonly PublicObjectCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(DataHolderSample), typeof(FieldSample)];

    private static readonly Type[] _InvalidTypes = Type.EmptyTypes;

    public PublicObjectCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal void Compare_DifferentObjectsDifferences(string value1, string value2)
    {
        var expected = new { Value = value1 };
        var actual = new { Value = value2 };

        DifferenceHintResult result = TestInstance.TryCompare(expected, actual, CreateChainer());

        result.HasData.Assert().Is(true);
        result.Data.Assert().IsNotEmpty();
    }
}
