using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.TesterTool.Validators;

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
}
