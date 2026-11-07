using System.Collections.Specialized;
using Werecodent.CreateAndFake.MutatorTool.Engine;
using Werecodent.CreateAndFake.MutatorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.MutatorTool.Handlers;

public sealed class StringDictionaryMutateHandlerTests()
    : AutoTester(typeof(StringDictionaryMutateHandler))
{
    protected override TesterMod OptionConfiguration { get; } =
        opt =>
            opt with
            {
                IgnorableExceptions = [typeof(InvalidCastException)],
                DisableParameterMutationTests = true,
            };

    [Fact]
    internal static void StringDictionaryMutateHandler_InternalOnly()
    {
        typeof(StringDictionaryMutateHandler).IsPublic.Assert().Is(false);
    }

    [Theory, RandomData]
    internal static void ModifySupported_EmptyWorks([Size(0)] StringDictionary data)
    {
        new StringDictionaryMutateHandler()
            .ModifySupported(data, new MutatorChainer(Tools.Mutator.Options, new MutatorEngine()))
            .Assert()
            .Is(true);
    }
}
