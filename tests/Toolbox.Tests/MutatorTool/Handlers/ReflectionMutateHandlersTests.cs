using Werecodent.CreateAndFake.MutatorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.MutatorTool.Handlers;

public static class ReflectionMutateHandlersTests
{
    [Fact]
    internal static void ReflectionMutateHandlers_InternalOnly()
    {
        typeof(ReflectionMutateHandlers).IsPublic.Assert().Is(false);
    }
}
