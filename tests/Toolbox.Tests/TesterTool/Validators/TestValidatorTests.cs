using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.TesterTool.Validators;

namespace Werecodent.CreateAndFake.Tests.TesterTool.Validators;

public static class TestValidatorTests
{
    [Fact]
    internal static Task TestValidator_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<TestValidator>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task TestValidator_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<TestValidator>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }
}
