namespace CreateAndFakeTests.Extensions;

public static class FakeExtensionsTests
{
    [Fact]
    internal static void FakeExtensions_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException(typeof(FakeExtensions));
    }

    [Fact]
    internal static void FakeExtensions_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation(typeof(FakeExtensions));
    }
}