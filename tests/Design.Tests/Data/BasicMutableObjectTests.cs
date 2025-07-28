using CreateAndFake.Design.Data;

namespace CreateAndFake.Design.Tests.Data;

public static class BasicMutableObjectTests
{
    [Fact]
    public static Task BasicMutableObject_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(typeof(BasicMutableObject));
    }

    [Fact]
    public static Task BasicMutableObject_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(typeof(BasicMutableObject));
    }
}
