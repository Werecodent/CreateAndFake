using Werecodent.CreateAndFake.MutatorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.MutatorTool.Handlers;

public sealed class ReflectionMutateHandlersTests() : AutoTester(typeof(ReflectionMutateHandlers))
{
    public override Task Tester_NoParameterMutation()
    {
        return Task.CompletedTask;
    }

    [Fact]
    internal static void ReflectionMutateHandlers_InternalOnly()
    {
        typeof(ReflectionMutateHandlers).IsPublic.Assert().Is(false);
    }
}
