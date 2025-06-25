using CreateAndFake.Design.Reiteration;

namespace CreateAndFake.Design.Tests.Reiteration;

public static class LimiterTests
{
    [Fact]
    internal static Task Limiter_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(Limiter.Few);
    }
}
