using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.TesterTool.Validators;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.Tests.TesterTool.Validators;

public sealed class EqualityValidatorTests() : AutoTester(typeof(EqualityValidator))
{
    protected override TesterMod OptionConfiguration { get; } =
        opt => opt with { IgnorableExceptions = [typeof(AssertException)] };

    [Theory, RandomData]
    internal static Task VerifyEqualsMatchesHashCodesAsync_ValidatesIValuerEquatable(
        IValuerEquatable sample
    )
    {
        return Tools.Tester.VerifyEqualsMatchesHashCodesAsync(
            sample.GetType(),
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static Task VerifyEqualsMatchesHashCodesAsync_ValidatesIValueEquatable(
        IValueEquatable sample
    )
    {
        return Tools.Tester.VerifyEqualsMatchesHashCodesAsync(
            sample.GetType(),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task VerifyEqualsMatchesHashCodesAsync_ValidatesObjects()
    {
        return Tools.Tester.VerifyEqualsMatchesHashCodesAsync<StructSample>(
            TestContext.Current.CancellationToken
        );
    }
}
