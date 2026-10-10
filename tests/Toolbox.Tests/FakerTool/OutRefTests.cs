using Werecodent.CreateAndFake.FakerTool;

namespace Werecodent.CreateAndFake.Tests.FakerTool;

public static class OutRefTests
{
    [Fact]
    internal static Task OutRef_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(OutRef<>),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task OutRef_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(OutRef<>),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(MemberAccessException)] }
        );
    }
}
