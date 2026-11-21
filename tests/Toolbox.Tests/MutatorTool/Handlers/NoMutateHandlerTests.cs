using Werecodent.CreateAndFake.MutatorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.MutatorTool.Handlers;

public sealed class NoMutateHandlerTests() : AutoTester(typeof(NoMutateHandler))
{
    protected override TesterMod OptionConfiguration { get; } =
        opt => opt with { DisableParameterMutationTests = true };

    [Fact]
    internal static void NoMutateHandler_InternalOnly()
    {
        typeof(NoMutateHandler).IsPublic.Assert().Is(false);
    }
}
