using Werecodent.CreateAndFake.Design.Content;

namespace Werecodent.CreateAndFake.Design.Tests.Content;

public static class CustomLockTests
{
    [Fact]
    internal static Task CustomLock_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<CustomLock>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task CustomLock_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<CustomLock>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task CustomLock_VerifyToolSupport()
    {
        return Tools.Tester.VerifyToolSetSupportAsync(
            typeof(CustomLock),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void CustomLock_VerifyValueEquality()
    {
        Tools.Tester.VerifyValueEquality<CustomLock>();
    }

    [Fact]
    internal static void CustomLock_Sealed()
    {
        typeof(CustomLock).IsSealed.Assert().Is(true);
    }
}
