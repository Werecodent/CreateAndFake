global using DuplicatorMod = System.Func<
    CreateAndFake.Toolbox.DuplicatorTool.DuplicatorOptions,
    CreateAndFake.Toolbox.DuplicatorTool.DuplicatorOptions>;

using CreateAndFake.Toolbox.DuplicatorTool;

namespace CreateAndFakeTests.Toolbox.DuplicatorTool;

public static class DuplicatorOptionsTests
{
    [Fact]
    internal static void DuplicatorOptions_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<DuplicatorOptions>();
    }

    [Fact]
    internal static void DuplicatorOptions_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<DuplicatorOptions>();
    }

    [Fact]
    internal static void DuplicatorOptions_ModFormRandomizable()
    {
        typeof(DuplicatorMod).CreateRandomInstance().Assert().IsNot(null);
    }
}