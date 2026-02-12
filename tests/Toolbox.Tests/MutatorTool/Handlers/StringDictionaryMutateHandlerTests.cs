using System.Collections.Specialized;
using CreateAndFake.MutatorTool.Engine;
using CreateAndFake.MutatorTool.Handlers;

namespace CreateAndFake.Tests.MutatorTool.Handlers;

public static class StringDictionaryMutateHandlerTests
{
    [Fact]
    internal static void StringDictionaryMutateHandler_InternalOnly()
    {
        typeof(StringDictionaryMutateHandler).IsPublic.Assert().Is(false);
    }

    [Theory, RandomData]
    internal static void StringDictionaryMutateHandler_EmptyWorks([Size(0)] StringDictionary data)
    {
        new StringDictionaryMutateHandler()
            .ModifySupported(data, new MutatorChainer(Tools.Mutator.Options, new MutatorEngine()))
            .Assert()
            .Is(true);
    }
}
