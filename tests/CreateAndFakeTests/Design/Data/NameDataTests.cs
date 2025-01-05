using CreateAndFake.Design.Data;

namespace CreateAndFakeTests.Design.Data;

public static class NameDataTests
{
    [Fact]
    public static void NameData_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException(typeof(NameData));
    }

    [Fact]
    public static void NameData_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation(typeof(NameData));
    }

    [Fact]
    internal static void Values_Populated()
    {
        NameData.Values.Assert().IsNotEmpty();
    }
}
