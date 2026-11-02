using System.Reflection;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.ValuerTool;
using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Engine;

public sealed class ValuerEngineTests : AutoTester<ValuerEngine>
{
    protected override TesterMod OptionConfiguration { get; } =
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
            };

    [Theory, RandomData]
    internal void Compare_NoHintsUnsupported(object expected, object actual)
    {
        TestInstance
            .Assert(x => x.Compare(expected, actual, CreateHintChainer(null)))
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal void Compare_NullResultSafe([Stub] ICompareHint hint, object expected, object actual)
    {
        TestInstance
            .Assert(x => x.Compare(expected, actual, CreateHintChainer(hint)))
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal void Compare_WithErrorHandling(
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

        TestInstance
            .Assert(x => x.Compare(expected, actual, CreateHintChainer(hint)).ToList())
            .Throws<ToolException>();
    }

    [Theory, RandomData]
    internal void CompareAsync_NoHintsUnsupported(object expected, object actual)
    {
        TestInstance
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
    internal void CompareAsync_NullResultSafe(
        [Stub] ICompareHint hint,
        object expected,
        object actual
    )
    {
        TestInstance
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
    internal void CompareAsync_WithErrorHandling(
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
                TestContext.Current.CancellationToken
            )
            .SetupReturn(Behavior<DifferenceHintAsyncResult>.Throw(error));

        TestInstance
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
    internal Task CompareAsync_WithErrorHandlingDuringYield(
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
                TestContext.Current.CancellationToken
            )
            .SetupReturn(new DifferenceHintAsyncResult(thrower()));

        return TestInstance
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
    internal void GetHashCode_NoHintsUnsupported(object item)
    {
        TestInstance
            .Assert(x => x.GetHashCode(item, CreateHintChainer(null)))
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal void GetHashCode_NullResultSafe([Stub] ICompareHint hint, object item)
    {
        TestInstance
            .Assert(x => x.GetHashCode(item, CreateHintChainer(hint)))
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal void GetHashCodeAsync_NoHintsUnsupported(object item)
    {
        TestInstance
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
    internal void GetHashCodeAsync_NullResultSafe([Stub] ICompareHint hint, object item)
    {
        TestInstance
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
    internal void GetHashCodeAsync_WithErrorHandling(
        [Stub] ICompareHint hint,
        object item,
        ArgumentException error
    )
    {
        hint.TryToAsyncGetHashCode(
                item,
                Arg.Any<IValuerChainer>(),
                TestContext.Current.CancellationToken
            )
            .SetupReturn(Behavior<HashCodeHintAsyncResult>.Throw(error));

        TestInstance
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
    internal Task GetHashCodeAsync_WithErrorHandlingDuringAwait(
        [Stub] ICompareHint hint,
        object item,
        ArgumentException error
    )
    {
        hint.TryToAsyncGetHashCode(
                item,
                Arg.Any<IValuerChainer>(),
                TestContext.Current.CancellationToken
            )
            .SetupReturn(new HashCodeHintAsyncResult(Task.FromException<int>(error)));

        return TestInstance
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
