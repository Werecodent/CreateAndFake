using Werecodent.CreateAndFake.MutatorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.MutatorTool.Handlers;

public static class NoMutateHandlerTests
{
    [Fact]
    internal static void NoMutateHandler_InternalOnly()
    {
        typeof(NoMutateHandler).IsPublic.Assert().Is(false);
    }
}
