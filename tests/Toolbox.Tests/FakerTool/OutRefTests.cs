using Werecodent.CreateAndFake.FakerTool;

namespace Werecodent.CreateAndFake.Tests.FakerTool;

public sealed class OutRefTests() : AutoTester(typeof(OutRef<>))
{
    protected override TesterMod OptionConfiguration { get; } =
        opt => opt with { IgnorableExceptions = [typeof(MemberAccessException)] };
}
