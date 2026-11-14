global using DuplicatorMod = System.Func<
    Werecodent.CreateAndFake.DuplicatorTool.DuplicatorOptions,
    Werecodent.CreateAndFake.DuplicatorTool.DuplicatorOptions
>;
using Werecodent.CreateAndFake.DuplicatorTool;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool;

public sealed class DuplicatorOptionsTests : AutoTester<DuplicatorOptions>
{
    protected override bool EqualsOverrideUsesFullValueEquality { get; } = false;

    [Fact]
    internal static void DuplicatorOptions_ModFormRandomizable()
    {
        typeof(DuplicatorMod).Tools().CreateRandomInstance().Assert().IsNotNull();
    }
}
