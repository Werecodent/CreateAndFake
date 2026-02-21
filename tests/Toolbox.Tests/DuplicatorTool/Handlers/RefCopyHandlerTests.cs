using CreateAndFake.DuplicatorTool.Handlers;

namespace CreateAndFake.Tests.DuplicatorTool.Handlers;

public static class RefCopyHandlerTests
{
    [Fact]
    internal static Task RefCopyHandler_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<RefCopyHandler>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task RefCopyHandler_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<RefCopyHandler>(
            TestContext.Current.CancellationToken
        );
    }
}
