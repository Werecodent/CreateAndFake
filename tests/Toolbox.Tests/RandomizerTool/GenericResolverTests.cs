using Werecodent.CreateAndFake.RandomizerTool;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool;

public static class GenericResolverTests
{
    [Fact]
    internal static Task GenericResolver_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(GenericResolver),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(ArgumentException)] }
        );
    }

    [Fact]
    internal static Task GenericResolver_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(GenericResolver),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(ArgumentException)] }
        );
    }
}
