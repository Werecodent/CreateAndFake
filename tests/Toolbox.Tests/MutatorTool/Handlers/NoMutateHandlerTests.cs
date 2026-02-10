using CreateAndFake.MutatorTool.Handlers;

namespace CreateAndFake.Tests.MutatorTool.Handlers;

public static class NoMutateHandlerTests
{
    [Fact]
    internal static void NoMutateHandler_InternalOnly()
    {
        typeof(NoMutateHandler).IsPublic.Assert().Is(false);
    }
}
