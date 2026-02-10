using CreateAndFake.MutatorTool.Handlers;

namespace CreateAndFake.Tests.MutatorTool.Handlers;

public static class StringDictionaryMutateHandlerTests
{
    [Fact]
    internal static void StringDictionaryMutateHandler_InternalOnly()
    {
        typeof(StringDictionaryMutateHandler).IsPublic.Assert().Is(false);
    }
}
