namespace CreateAndFake.Tests.Fluent;

public static class ExceptionChainerTests
{
    [Fact]
    internal static Task ExceptionChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(
            typeof(ExceptionChainer<Exception>),
            TestContext.Current.CancellationToken
        );
    }
}
