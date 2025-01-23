namespace CreateAndFakeTests.Extensions;

public static class CreateExtensionsTests
{
    [Fact]
    internal static void CreateExtensions_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException(typeof(CreateExtensions));
    }

    [Fact]
    internal static void CreateExtensions_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation(typeof(CreateExtensions));
    }
}