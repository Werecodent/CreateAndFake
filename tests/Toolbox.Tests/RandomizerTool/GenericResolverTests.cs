using Werecodent.CreateAndFake.RandomizerTool;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool;

public sealed class GenericResolverTests() : AutoTester(typeof(GenericResolver))
{
    protected override TesterMod OptionConfiguration { get; } =
        opt => opt with { IgnorableExceptions = [typeof(ArgumentException)] };
}
