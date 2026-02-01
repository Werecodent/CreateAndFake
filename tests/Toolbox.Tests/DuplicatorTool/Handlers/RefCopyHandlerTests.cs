using CreateAndFake.DuplicatorTool.Handlers;

namespace CreateAndFake.Tests.DuplicatorTool.Handlers;

public static class RefCopyHandlerTests
{
    [Fact]
    internal static Task RefCopyHandler_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<RefCopyHandler>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task RefCopyHandler_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<RefCopyHandler>(
            TestContext.Current.CancellationToken
        );
    }
}
