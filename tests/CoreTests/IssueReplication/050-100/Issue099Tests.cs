using CreateAndFake.Toolbox.FakerTool;

namespace CreateAndFakeTests.IssueReplication;

public static class Issue099Tests
{
    public static int ExtendedCall(this ITester sample, object item)
    {
        return sample?.GetValue(item) ?? 0;
    }

    public interface ITester
    {
        int GetValue(object item);
    }

    [Theory, RandomData]
    internal static void Issue099_FakingThrowsWithExtensionMethod(Fake<ITester> sample, object item, int result)
    {
        Tools.Asserter.Throws<NotSupportedException>(
            () => sample.Setup(f => f.ExtendedCall(item), Behavior.Returns(result)));
    }

    [Theory, RandomData]
    internal static void Issue099_FluentPassthroughWithExtensionMethod([Stub] ITester sample, object item, int result)
    {
        sample.ExtendedCall(item).SetupReturn(result);
        sample.ExtendedCall(item).Assert().Is(result);
    }
}
