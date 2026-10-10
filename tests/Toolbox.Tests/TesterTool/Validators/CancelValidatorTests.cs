using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.TesterTool.Validators;

namespace Werecodent.CreateAndFake.Tests.TesterTool.Validators;

public static class CancelValidatorTests
{
    [Fact]
    internal static Task CancelValidator_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<CancelValidator>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Fact]
    internal static Task CancelValidator_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<CancelValidator>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }
}
