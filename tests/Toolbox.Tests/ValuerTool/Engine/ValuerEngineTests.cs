using System.Reflection;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.ValuerTool;
using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Engine;

public static class ValuerEngineTests
{
    private static readonly ValuerEngine _TestInstance = new();

    [Fact]
    internal static Task ValuerEngine_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ValuerEngine>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions = [typeof(UnsupportedException), typeof(ToolException)],
                }
        );
    }

    [Fact]
    internal static Task ValuerEngine_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ValuerEngine>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    InjectionValues = [Tools.Valuer.Options],
                    IgnorableExceptions =
                    [
                        typeof(UnsupportedException),
                        typeof(TargetException),
                        typeof(ToolException),
                    ],
                }
        );
    }

    [Theory, RandomData]
    internal static void Compare_NoHintsUnsupported(object expected, object actual)
    {
        _TestInstance
            .Assert(x => x.Compare(expected, actual, CreateHintChainer(null)))
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal static void Compare_NullResultSafe(
        [Stub] ICompareHint hint,
        object expected,
        object actual
    )
    {
        _TestInstance
            .Assert(x => x.Compare(expected, actual, CreateHintChainer(hint)))
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal static void Compare_WithErrorHandling(
        [Stub] ICompareHint hint,
        object expected,
        object actual,
        ArgumentException error
    )
    {
        IEnumerable<Difference> thrower()
        {
            yield return new Difference("");
            throw error;
        }

        hint.TryToCompare(expected, actual, Arg.Any<IValuerChainer>())
            .SetupReturn(new DifferenceHintResult(thrower()));

        _TestInstance
            .Assert(x => x.Compare(expected, actual, CreateHintChainer(hint)).ToList())
            .Throws<ToolException>();
    }

    [Theory, RandomData]
    internal static void CompareAsync_NoHintsUnsupported(object expected, object actual)
    {
        _TestInstance
            .Assert(x =>
                x.CompareAsync(
                    expected,
                    actual,
                    CreateHintChainer(null),
                    TestContext.Current.CancellationToken
                )
            )
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal static void CompareAsync_NullResultSafe(
        [Stub] ICompareHint hint,
        object expected,
        object actual
    )
    {
        _TestInstance
            .Assert(x =>
                x.CompareAsync(
                    expected,
                    actual,
                    CreateHintChainer(hint),
                    TestContext.Current.CancellationToken
                )
            )
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal static void CompareAsync_WithErrorHandling(
        [Stub] ICompareHint hint,
        object expected,
        object actual,
        ArgumentException error
    )
    {
        hint.TryToAsyncCompare(
                expected,
                actual,
                Arg.Any<IValuerChainer>(),
                Arg.Any<CancellationToken>()
            )
            .SetupReturn(Behavior<DifferenceHintAsyncResult>.Throw(error));

        _TestInstance
            .Assert(x =>
                x.CompareAsync(
                    expected,
                    actual,
                    CreateHintChainer(hint),
                    TestContext.Current.CancellationToken
                )
            )
            .Throws<ToolException>();
    }

    [Theory, RandomData]
    internal static Task CompareAsync_WithErrorHandlingDuringYield(
        [Stub] ICompareHint hint,
        object expected,
        object actual,
        ArgumentException error
    )
    {
        async IAsyncEnumerable<Difference> thrower()
        {
            await Task.Delay(0, TestContext.Current.CancellationToken);
            yield return new Difference("");
            throw error;
        }

        hint.TryToAsyncCompare(
                expected,
                actual,
                Arg.Any<IValuerChainer>(),
                Arg.Any<CancellationToken>()
            )
            .SetupReturn(new DifferenceHintAsyncResult(thrower()));

        return _TestInstance
            .CompareAsync(
                expected,
                actual,
                CreateHintChainer(hint),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsAsync<ToolException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static void GetHashCode_NoHintsUnsupported(object item)
    {
        _TestInstance
            .Assert(x => x.GetHashCode(item, CreateHintChainer(null)))
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal static void GetHashCode_NullResultSafe([Stub] ICompareHint hint, object item)
    {
        _TestInstance
            .Assert(x => x.GetHashCode(item, CreateHintChainer(hint)))
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal static void GetHashCodeAsync_NoHintsUnsupported(object item)
    {
        _TestInstance
            .Assert(x =>
                x.GetHashCodeAsync(
                    item,
                    CreateHintChainer(null),
                    TestContext.Current.CancellationToken
                )
            )
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal static void GetHashCodeAsync_NullResultSafe([Stub] ICompareHint hint, object item)
    {
        _TestInstance
            .Assert(x =>
                x.GetHashCodeAsync(
                    item,
                    CreateHintChainer(hint),
                    TestContext.Current.CancellationToken
                )
            )
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal static void GetHashCodeAsync_WithErrorHandling(
        [Stub] ICompareHint hint,
        object item,
        ArgumentException error
    )
    {
        hint.TryToAsyncGetHashCode(item, Arg.Any<IValuerChainer>(), Arg.Any<CancellationToken>())
            .SetupReturn(Behavior<HashCodeHintAsyncResult>.Throw(error));

        _TestInstance
            .Assert(x =>
                x.GetHashCodeAsync(
                    item,
                    CreateHintChainer(hint),
                    TestContext.Current.CancellationToken
                )
            )
            .Throws<ToolException>();
    }

    [Theory, RandomData]
    internal static Task GetHashCodeAsync_WithErrorHandlingDuringAwait(
        [Stub] ICompareHint hint,
        object item,
        ArgumentException error
    )
    {
        hint.TryToAsyncGetHashCode(item, Arg.Any<IValuerChainer>(), Arg.Any<CancellationToken>())
            .SetupReturn(new HashCodeHintAsyncResult(Task.FromException<int>(error)));

        return _TestInstance
            .GetHashCodeAsync(item, CreateHintChainer(hint), TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<ToolException>(TestContext.Current.CancellationToken);
    }

    private static ValuerChainer CreateHintChainer(ICompareHint hint)
    {
        return new ValuerChainer(
            Tools.Valuer.Options with
            {
                IncludeFoundHints = false,
                IncludeFrameworkHints = false,
                Hints = hint != null ? [hint] : [],
            },
            new ValuerEngine()
        );
    }
}
