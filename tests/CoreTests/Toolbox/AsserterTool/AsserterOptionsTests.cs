global using AsserterMod = System.Func<
    CreateAndFake.Toolbox.AsserterTool.AsserterOptions,
    CreateAndFake.Toolbox.AsserterTool.AsserterOptions>;

using CreateAndFake.Toolbox.AsserterTool;

namespace CreateAndFakeTests.Toolbox.AsserterTool;

public static class AsserterOptionsTests
{
    [Fact]
    internal static void AsserterOptions_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<AsserterOptions>();
    }

    [Fact]
    internal static void AsserterOptions_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<AsserterOptions>();
    }

    [Fact]
    internal static void AsserterOptions_ModFormRandomizable()
    {
        typeof(AsserterMod).CreateRandomInstance().Assert().IsNot(null);
    }
}