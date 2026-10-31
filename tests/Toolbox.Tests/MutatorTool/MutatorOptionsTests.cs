global using MutatorMod = System.Func<
    Werecodent.CreateAndFake.MutatorTool.MutatorOptions,
    Werecodent.CreateAndFake.MutatorTool.MutatorOptions
>;
using Werecodent.CreateAndFake.MutatorTool;

namespace Werecodent.CreateAndFake.Tests.MutatorTool;

public static class MutatorOptionsTests
{
    [Fact]
    internal static Task MutatorOptions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<MutatorOptions>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task MutatorOptions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<MutatorOptions>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void MutatorOptions_ModFormRandomizable()
    {
        typeof(MutatorMod).Tools().CreateRandomInstance().Assert().IsNotNull();
    }
}
