global using AsserterMod = System.Func<
    CreateAndFake.AsserterTool.AsserterOptions,
    CreateAndFake.AsserterTool.AsserterOptions>;

using CreateAndFake.AsserterTool;

namespace CreateAndFake.Tests.AsserterTool;

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