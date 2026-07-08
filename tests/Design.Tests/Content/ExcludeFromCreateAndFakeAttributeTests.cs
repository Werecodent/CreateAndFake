using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Tests.Content;

public static class ExcludeFromCreateAndFakeAttributeTests
{
    [Fact]
    internal static Task ExcludeFromCreateAndFakeAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ExcludeFromCreateAndFakeAttribute>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task ExcludeFromCreateAndFakeAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ExcludeFromCreateAndFakeAttribute>(
            TestContext.Current.CancellationToken
        );
    }
}
