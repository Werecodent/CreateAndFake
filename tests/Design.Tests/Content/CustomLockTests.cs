using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Tests.Content;

public static class CustomLockTests
{
    [Fact]
    internal static Task CustomLock_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<CustomLock>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task CustomLock_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<CustomLock>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void CustomLock_Sealed()
    {
        typeof(CustomLock).IsSealed.Assert().Is(true);
    }
}
