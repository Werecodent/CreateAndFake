using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.TesterTool.Validators;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.Tests.TesterTool.Validators;

public static class EqualityValidatorTests
{
    private static readonly EqualityValidator _TestInstance = new(Tools.Tester.Options);

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
    internal static void VerifyEqualsMatchesHashCodes_ValidatesIValuerEquatable(
        IValuerEquatable sample
    )
    {
        _TestInstance.VerifyEqualsMatchesHashCodes(sample.GetType());
    }

    [Theory, RandomData]
    internal static void VerifyEqualsMatchesHashCodes_ValidatesIValueEquatable(
        IValueEquatable sample
    )
    {
        _TestInstance.VerifyEqualsMatchesHashCodes(sample.GetType());
    }

    [Fact]
    internal static void VerifyEqualsMatchesHashCodes_ValidatesObjects()
    {
        _TestInstance.VerifyEqualsMatchesHashCodes(typeof(Times));
    }
}
