using System.Collections.Frozen;
using CreateAndFake.Design.Reiteration;

namespace CreateAndFake.Design.Tests.Reiteration;

public static class LimiterTests
{
    private static readonly FrozenSet<Type> ignorableExceptions =
    [
        typeof(NotSupportedException),
        typeof(TimeoutException),
        typeof(FormatException),
    ];

    [Fact]
    internal static Task Limiter_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(
            Limiter.Few,
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = ignorableExceptions }
        );
    }

    /*[Fact]
    internal static Task Limiter_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(
            Limiter.Few,
            opt => opt with { IgnorableExceptions = ignorableExceptions }
        );
    }*/
}
