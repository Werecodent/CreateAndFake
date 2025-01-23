global using FakerMod = System.Func<
    CreateAndFake.Toolbox.FakerTool.FakerOptions,
    CreateAndFake.Toolbox.FakerTool.FakerOptions>;

using CreateAndFake.Toolbox.FakerTool;

namespace CreateAndFakeTests.Toolbox.FakerTool;

public static class FakerOptionsTests
{
    [Fact]
    internal static void FakerOptions_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<FakerOptions>();
    }

    [Fact]
    internal static void FakerOptions_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<FakerOptions>();
    }

    [Fact]
    internal static void FakerOptions_ModFormRandomizable()
    {
        typeof(FakerMod).CreateRandomInstance().Assert().IsNot(null);
    }
}