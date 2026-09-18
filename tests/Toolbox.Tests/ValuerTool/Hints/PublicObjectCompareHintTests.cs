using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.ValuerTool.Engine;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class PublicObjectCompareHintTests : CompareHintTestBase<PublicObjectCompareHint>
{
    private static readonly PublicObjectCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(DataHolderSample), typeof(FieldSample)];

    private static readonly Type[] _InvalidTypes = Type.EmptyTypes;

    public PublicObjectCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Fact]
    public override Task CompareHint_NoParameterMutation()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<PublicObjectCompareHint>(
            TestContext.Current.CancellationToken,
            opt => opt with { InjectionValues = [Tools.Randomizer.Create<DataHolderSample>()] }
        );
    }

    [Theory, RandomData]
    internal void Compare_DifferentObjectsDifferences(string value1, string value2)
    {
        var expected = new { Value = value1 };
        var actual = new { Value = value2 };

        DifferenceHintResult result = TestInstance.TryToCompare(expected, actual, CreateChainer());

        result.HasData.Assert().Is(true);
        result.Data.Assert().IsNotEmpty();
    }
}
