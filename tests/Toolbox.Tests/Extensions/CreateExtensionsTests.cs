namespace CreateAndFake.Tests.Extensions;

public static class CreateExtensionsTests
{
    [Fact]
    internal static Task CreateExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(typeof(CreateExtensions));
    }

    [Fact]
    internal static Task CreateExtensions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(typeof(CreateExtensions));
    }
}
