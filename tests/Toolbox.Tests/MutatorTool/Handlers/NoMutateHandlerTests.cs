using Werecodent.CreateAndFake.MutatorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.MutatorTool.Handlers;

public sealed class NoMutateHandlerTests() : AutoTester(typeof(NoMutateHandler))
{
    public override Task Tester_NoParameterMutation()
    {
        return Task.CompletedTask;
    }

    [Fact]
    internal static void NoMutateHandler_InternalOnly()
    {
        typeof(NoMutateHandler).IsPublic.Assert().Is(false);
    }
}
