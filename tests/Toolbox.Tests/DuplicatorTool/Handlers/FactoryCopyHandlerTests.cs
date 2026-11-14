using Werecodent.CreateAndFake.DuplicatorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Handlers;

public sealed class FactoryCopyHandlerTests() : AutoTester(typeof(FactoryCopyHandler<>))
{
    protected override TesterMod OptionConfiguration { get; } =
        opt => opt with { IgnorableExceptions = [typeof(InvalidCastException)] };
}
