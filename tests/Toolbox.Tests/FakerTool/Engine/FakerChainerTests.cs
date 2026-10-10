using Werecodent.CreateAndFake.FakerTool.Engine;

namespace Werecodent.CreateAndFake.Tests.FakerTool.Engine;

public static class FakerChainerTests
{
    [Fact]
    internal static Task FakerChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<FakerChainer>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions =
                    [
                        typeof(ArgumentException),
                        typeof(InvalidOperationException),
                        typeof(ArgumentOutOfRangeException),
                    ],
                    MethodsToIgnore =
                    [
                        nameof(FakerChainer.InjectMocks),
                        nameof(FakerChainer.InjectStubs),
                    ],
                }
        );
    }
}
