global using DuplicatorMod = System.Func<
    Werecodent.CreateAndFake.DuplicatorTool.DuplicatorOptions,
    Werecodent.CreateAndFake.DuplicatorTool.DuplicatorOptions
>;
using Werecodent.CreateAndFake.DuplicatorTool;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool;

public static class DuplicatorOptionsTests
{
    [Fact]
    internal static Task DuplicatorOptions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<DuplicatorOptions>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task DuplicatorOptions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<DuplicatorOptions>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void DuplicatorOptions_ModFormRandomizable()
    {
        typeof(DuplicatorMod).Tools().CreateRandomInstance().Assert().IsNotNull();
    }
}
