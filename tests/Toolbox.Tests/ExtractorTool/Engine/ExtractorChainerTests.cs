using System.Reflection;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.ExtractorTool.Engine;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Engine;

public static class ExtractorChainerTests
{
    private static readonly TesterMod _Config = opt =>
        opt with
        {
            IgnorableExceptions =
            [
                typeof(UnsupportedException),
                typeof(ToolException),
                typeof(TargetParameterCountException),
                typeof(MismatchedAccessException),
            ],
        };

    [Fact]
    internal static Task ExtractorChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ExtractorChainer>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task ExtractorChainer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ExtractorChainer>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Theory, RandomData]
    internal static async Task AddFoundValue_CannotUseAfterAsyncCall(
        ExtractorChainer chainer,
        object item1,
        object item2
    )
    {
        await chainer
            .AddFoundValueAsync(item1, TestContext.Current.CancellationToken)
            .Assert()
            .HasResultAsync(true, TestContext.Current.CancellationToken);

        chainer.Assert(x => x.AddFoundValue(item2)).Throws<MismatchedAccessException>();
    }

#pragma warning disable MA0042, VSTHRD103 // Behavior specifically being tested.

    [Theory, RandomData]
    internal static Task AddFoundValueAsync_CannotUseAfterSyncCall(
        ExtractorChainer chainer,
        object item1,
        object item2
    )
    {
        chainer.AddFoundValue(item1).Assert().Is(true);

        return chainer
            .AddFoundValueAsync(item2, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<MismatchedAccessException>(TestContext.Current.CancellationToken);
    }

#pragma warning restore
}
