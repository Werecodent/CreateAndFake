using CreateAndFake.Design.Data;

namespace CreateAndFake.Design.Tests.Data;

public static class NameDataTests
{
    [Fact]
    public static Task NameData_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(typeof(NameData));
    }

    [Fact]
    public static Task NameData_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(typeof(NameData));
    }

    [Fact]
    internal static void Values_Populated()
    {
        NameData.Values.Assert().IsNotEmpty();
    }
}
