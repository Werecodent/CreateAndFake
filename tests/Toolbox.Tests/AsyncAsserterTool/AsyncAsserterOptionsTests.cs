global using AsyncAsserterMod = System.Func<
    CreateAndFake.AsyncAsserterTool.AsyncAsserterOptions,
    CreateAndFake.AsyncAsserterTool.AsyncAsserterOptions
>;
using CreateAndFake.AsyncAsserterTool;

namespace CreateAndFake.Tests.AsyncAsserterTool;

public static class AsyncAsserterOptionsTests
{
    [Fact]
    internal static Task AsyncAsserterOptions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<AsyncAsserterOptions>();
    }

    [Fact]
    internal static Task AsyncAsserterOptions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<AsyncAsserterOptions>();
    }

    [Fact]
    internal static void AsyncAsserterOptions_ModFormRandomizable()
    {
        typeof(AsyncAsserterMod).CreateRandomInstance().Assert().IsNot(null);
    }
}
