namespace CreateAndFake.Tests.Extensions;

public static class FakeExtensionsTests
{
    [Fact]
    internal static Task FakeExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(typeof(FakeExtensions));
    }

    [Fact]
    internal static Task FakeExtensions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(typeof(FakeExtensions));
    }
}
