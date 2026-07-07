using CreateAndFake.AsserterTool;
using CreateAndFake.TesterTool.Validators;

namespace CreateAndFake.Tests.TesterTool.Validators;

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
