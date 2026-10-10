using Werecodent.CreateAndFake.MutatorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.MutatorTool.Handlers;

public static class FactoryMutateHandlerTests
{
    [Fact]
    internal static void FactoryMutateHandler_InternalOnly()
    {
        typeof(FactoryMutateHandler<>).IsPublic.Assert().Is(false);
    }
}
