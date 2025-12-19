using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Tests.Content;

public static class AsyncEnumHelperTests
{
    [Fact]
    internal static Task AsyncEnumHelper_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(typeof(AsyncEnumHelper));
    }

    [Fact]
    internal static Task AsyncEnumHelper_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(typeof(AsyncEnumHelper));
    }
}
