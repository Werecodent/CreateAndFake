global using MutatorMod = System.Func<
    CreateAndFake.Toolbox.MutatorTool.MutatorOptions,
    CreateAndFake.Toolbox.MutatorTool.MutatorOptions>;

using CreateAndFake.Toolbox.MutatorTool;

namespace CreateAndFakeTests.Toolbox.MutatorTool;

public static class MutatorOptionsTests
{
    [Fact]
    internal static void MutatorOptions_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<MutatorOptions>();
    }

    [Fact]
    internal static void MutatorOptions_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<MutatorOptions>();
    }

    [Fact]
    internal static void MutatorOptions_ModFormRandomizable()
    {
        typeof(MutatorMod).CreateRandomInstance().Assert().IsNot(null);
    }
}