using Werecodent.CreateAndFake.MutatorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.MutatorTool.Handlers;

public sealed class ReflectionMutateHandlersTests() : AutoTester(typeof(ReflectionMutateHandlers))
{
    protected override TesterMod OptionConfiguration { get; } =
        opt => opt with { DisableParameterMutationTests = true };

    [Fact]
    internal static void ReflectionMutateHandlers_InternalOnly()
    {
        typeof(ReflectionMutateHandlers).IsPublic.Assert().Is(false);
    }
}
