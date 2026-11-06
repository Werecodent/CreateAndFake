using Werecodent.CreateAndFake.Design.Content;

namespace Werecodent.CreateAndFake.Design.Tests.Content;

public sealed class CustomLockTests : AutoTester<CustomLock>
{
    [Fact]
    internal static Task CustomLock_VerifyToolSupport()
    {
        return Tools.Tester.VerifyToolSetSupportAsync(
            typeof(CustomLock),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void CustomLock_Sealed()
    {
        typeof(CustomLock).IsSealed.Assert().Is(true);
    }
}
