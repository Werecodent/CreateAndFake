using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.TesterTool.Validators;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.Tests.TesterTool.Validators;

public static class EqualityValidatorTests
{
    [Fact]
    internal static Task EqualityValidator_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<EqualityValidator>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task EqualityValidator_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<EqualityValidator>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

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
