global using MutatorMod = System.Func<
    Werecodent.CreateAndFake.MutatorTool.MutatorOptions,
    Werecodent.CreateAndFake.MutatorTool.MutatorOptions
>;
using Werecodent.CreateAndFake.MutatorTool;

namespace Werecodent.CreateAndFake.Tests.MutatorTool;

public sealed class MutatorOptionsTests : AutoTester<MutatorOptions>
{
    protected override bool EqualsOverrideUsesFullValueEquality { get; } = false;

    [Fact]
    internal static void MutatorOptions_ModFormRandomizable()
    {
        typeof(MutatorMod).Tools().CreateRandomInstance().Assert().IsNotNull();
    }
}
