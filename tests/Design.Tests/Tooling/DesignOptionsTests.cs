using Werecodent.CreateAndFake.Design.Tooling;

namespace Werecodent.CreateAndFake.Design.Tests.Tooling;

public sealed class DesignOptionsTests : AutoTester<DesignOptions>
{
    protected override bool EqualsOverrideUsesFullValueEquality { get; } = false;
}
