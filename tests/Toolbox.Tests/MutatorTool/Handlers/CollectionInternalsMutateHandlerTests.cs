using Werecodent.CreateAndFake.MutatorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.MutatorTool.Handlers;

public sealed class CollectionInternalsMutateHandlerTests()
    : AutoTester(typeof(CollectionInternalsMutateHandler))
{
    protected override TesterMod OptionConfiguration { get; } =
        opt => opt with { IgnorableExceptions = [typeof(InvalidCastException)] };

    public override Task Tester_NoParameterMutation()
    {
        return Task.CompletedTask;
    }

    [Fact]
    internal static void CollectionInternalsMutateHandler_InternalOnly()
    {
        typeof(CollectionInternalsMutateHandler).IsPublic.Assert().Is(false);
    }

    [Fact]
    internal static void SupportedType_SetAsInvalid()
    {
        new CollectionInternalsMutateHandler().SupportedType.Assert().IsNull();
    }
}
