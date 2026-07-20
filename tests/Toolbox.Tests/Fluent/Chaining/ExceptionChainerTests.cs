using CreateAndFake.Fluent.Chaining;

namespace CreateAndFake.Tests.Fluent.Chaining;

public static class ExceptionChainerTests
{
    [Fact]
    internal static Task ExceptionChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(ExceptionChainer<>),
            TestContext.Current.CancellationToken
        );
    }
}
