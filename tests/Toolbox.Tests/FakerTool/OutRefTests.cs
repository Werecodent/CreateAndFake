using CreateAndFake.FakerTool;

namespace CreateAndFake.Tests.FakerTool;

public static class OutRefTests
{
    [Fact]
    internal static Task OutRef_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(typeof(OutRef<>));
    }

    [Fact]
    internal static Task OutRef_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(typeof(OutRef<>));
    }
}
