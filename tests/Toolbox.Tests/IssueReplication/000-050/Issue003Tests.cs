using CreateAndFake.FakerTool;

namespace CreateAndFake.Tests.IssueReplication;

public static class Issue003Tests
{
    public abstract class RefHolder
    {
        public abstract int TestMethodA(out int value);

        public abstract int TestMethodB(ref int value);
    }

    [Theory, RandomData]
    internal static void Issue003_FakeHandlesOut(Fake<RefHolder> sample, int testValue, int result)
    {
        sample.Setup(
            d => d.TestMethodA(out Arg.AnyRef<int>().Var),
            Behavior.Call(
                (OutRef<int> v) =>
                {
                    v.Var = testValue;
                    return result;
                }
            )
        );

        sample.Dummy.TestMethodA(out int newValue).Assert().Is(result);
        sample.Verify();
        newValue.Assert().Is(testValue);
    }

    [Theory, RandomData]
    internal static void Issue003_FakeHandlesRef(Fake<RefHolder> sample, int result)
    {
        int testValue = 0;
        sample.Setup(
            d => d.TestMethodB(ref Arg.AnyRef<int>().Var),
            Behavior.Call(
                (OutRef<int> v) =>
                {
                    v.Var -= 5;
                    return result;
                }
            )
        );

        sample.Dummy.TestMethodB(ref testValue).Assert().Is(result);
        sample.Verify();
        testValue.Assert().Is(-5);
    }
}
