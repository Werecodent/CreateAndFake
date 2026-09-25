namespace Werecodent.CreateAndFake.NUnit.v3.Tests;

[TestFixture]
public static class MethodWrapperCreateHintTests
{
    [Test]
    public static Task MethodWrapperCreateHint_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<MethodWrapperCreateHint>(
            TestContext.CurrentContext.CancellationToken
        );
    }

    [Test]
    public static Task MethodWrapperCreateHint_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<MethodWrapperCreateHint>(
            TestContext.CurrentContext.CancellationToken
        );
    }
}
