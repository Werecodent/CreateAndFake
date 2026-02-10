using CreateAndFake.MutatorTool.Handlers;

namespace CreateAndFake.Tests.MutatorTool.Handlers;

public static class CollectionInternalsMutateHandlerTests
{
    [Fact]
    internal static void CollectionInternalsMutateHandler_InternalOnly()
    {
        typeof(CollectionInternalsMutateHandler).IsPublic.Assert().Is(false);
    }
}
