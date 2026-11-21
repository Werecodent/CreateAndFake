using Werecodent.CreateAndFake.MutatorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.MutatorTool.Handlers;

public sealed class CollectionInternalsMutateHandlerTests()
    : AutoTester(typeof(CollectionInternalsMutateHandler))
{
    protected override TesterMod OptionConfiguration { get; } =
        opt =>
            opt with
            {
                IgnorableExceptions = [typeof(InvalidCastException)],
                DisableParameterMutationTests = true,
            };

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
