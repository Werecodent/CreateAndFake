global using MutatorMod = System.Func<
    CreateAndFake.MutatorTool.MutatorOptions,
    CreateAndFake.MutatorTool.MutatorOptions
>;
using CreateAndFake.MutatorTool;

namespace CreateAndFake.Tests.MutatorTool;

public static class MutatorOptionsTests
{
    [Fact]
    internal static Task MutatorOptions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<MutatorOptions>();
    }

    [Fact]
    internal static Task MutatorOptions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<MutatorOptions>();
    }

    [Fact]
    internal static void MutatorOptions_ModFormRandomizable()
    {
        typeof(MutatorMod).CreateRandomInstance().Assert().IsNot(null);
    }
}
