using CreateAndFake.Toolbox.FakerTool;

namespace CreateAndFakeTests.Toolbox.FakerTool;

public static class OutRefTests
{
    [Fact]
    internal static void OutRef_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException(typeof(OutRef<>));
    }

    [Fact]
    internal static void OutRef_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation(typeof(OutRef<>));
    }
}