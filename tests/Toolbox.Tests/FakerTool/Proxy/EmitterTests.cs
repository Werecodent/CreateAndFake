using Werecodent.CreateAndFake.FakerTool.Proxy;

namespace Werecodent.CreateAndFake.Tests.FakerTool.Proxy;

public sealed class EmitterTests() : AutoTester(typeof(Emitter))
{
    protected override TesterMod OptionConfiguration { get; } =
        opt => opt with { DisableParameterMutationTests = true };
}
