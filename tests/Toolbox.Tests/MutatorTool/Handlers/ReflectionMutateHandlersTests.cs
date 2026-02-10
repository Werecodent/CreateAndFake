using CreateAndFake.MutatorTool.Handlers;

namespace CreateAndFake.Tests.MutatorTool.Handlers;

public static class ReflectionMutateHandlersTests
{
    [Fact]
    internal static void ReflectionMutateHandlers_InternalOnly()
    {
        typeof(ReflectionMutateHandlers).IsPublic.Assert().Is(false);
    }
}
