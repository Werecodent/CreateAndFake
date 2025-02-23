global using FakerMod = System.Func<
    CreateAndFake.FakerTool.FakerOptions,
    CreateAndFake.FakerTool.FakerOptions>;

using CreateAndFake.FakerTool;

namespace CreateAndFake.Tests.FakerTool;

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