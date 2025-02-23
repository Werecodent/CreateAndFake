global using DuplicatorMod = System.Func<
    CreateAndFake.DuplicatorTool.DuplicatorOptions,
    CreateAndFake.DuplicatorTool.DuplicatorOptions>;

using CreateAndFake.DuplicatorTool;

namespace CreateAndFake.Tests.DuplicatorTool;

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