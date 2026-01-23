using CreateAndFake.Design.Content;
using CreateAndFake.FakerTool.Proxy;

namespace CreateAndFake.Design.Tests.Content;

public static class TypeSupporterTests
{
    [Fact]
    internal static Task TypeSupporter_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(
            typeof(TypeSupporter),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task TypeSupporter_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(
            typeof(TypeSupporter),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions =
                    [
                        typeof(ArgumentNullException),
                        typeof(ArgumentException),
                        typeof(FakeCallException),
                    ],
                }
        );
    }
}
