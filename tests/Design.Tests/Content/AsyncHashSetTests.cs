using Werecodent.CreateAndFake.Design.Content;

namespace Werecodent.CreateAndFake.Design.Tests.Content;

public static class AsyncHashSetTests
{
    [Fact]
    internal static Task AsyncHashSet_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(AsyncHashSet<>),
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static Task AsyncHashSet_NoParameterMutation([Cap(7, 9)] int iterationLimit)
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(AsyncHashSet<>),
            TestContext.Current.CancellationToken,
            opt => opt with { InjectionValues = [iterationLimit] }
        );
    }
}
