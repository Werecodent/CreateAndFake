namespace CreateAndFakeTests.Extensions;

public static class FakeExtensionsTests
{
    [Fact]
    internal static void FakeExtensions_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException(typeof(FakeExtensions));
    }
}