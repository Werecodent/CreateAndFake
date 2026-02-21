using CreateAndFake.Design.Data;

namespace CreateAndFake.Design.Tests.Data;

public static class NameDataTests
{
    [Fact]
    public static Task NameData_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(NameData),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task NameData_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(NameData),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void Values_Populated()
    {
        NameData.Values.Assert().IsNotEmpty();
    }
}
