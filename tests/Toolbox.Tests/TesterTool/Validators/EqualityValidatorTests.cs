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
    internal static void VerifyEqualsMatchesHashCodes_ValidatesIValuerEquatable(
        IValuerEquatable sample
    )
    {
        Tools.Tester.VerifyEqualsMatchesHashCodes(sample.GetType());
    }

    [Theory, RandomData]
    internal static void VerifyEqualsMatchesHashCodes_ValidatesIValueEquatable(
        IValueEquatable sample
    )
    {
        Tools.Tester.VerifyEqualsMatchesHashCodes(sample.GetType());
    }

    [Fact]
    internal static void VerifyEqualsMatchesHashCodes_ValidatesObjects()
    {
        Tools.Tester.VerifyEqualsMatchesHashCodes<StructSample>();
    }
}
