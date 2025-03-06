global using MutatorMod = System.Func<
    CreateAndFake.MutatorTool.MutatorOptions,
    CreateAndFake.MutatorTool.MutatorOptions
>;
using CreateAndFake.MutatorTool;

namespace CreateAndFake.Tests.MutatorTool;

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
