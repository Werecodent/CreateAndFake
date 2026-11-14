using Werecodent.CreateAndFake.MutatorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.MutatorTool.Handlers;

public sealed class FactoryMutateHandlerTests() : AutoTester(typeof(FactoryMutateHandler<>))
{
    protected override TesterMod OptionConfiguration { get; } =
        opt =>
            opt with
            {
                IgnorableExceptions = [typeof(InvalidCastException)],
                DisableParameterMutationTests = true,
            };

    [Fact]
    internal static void FactoryMutateHandler_InternalOnly()
    {
        typeof(FactoryMutateHandler<>).IsPublic.Assert().Is(false);
    }
}
