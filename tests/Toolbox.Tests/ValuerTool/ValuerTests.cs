using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Types;
using CreateAndFake.FakerTool;
using CreateAndFake.ValuerTool;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.Tests.ValuerTool;

public static class ValuerTests
{
    private static readonly TesterMod config = opt =>
        opt with
        {
            IgnorableExceptions =
            [
                typeof(ToolException),
                typeof(TimeoutException),
                typeof(UnsupportedException),
            ],
        };

    [Fact]
    internal static Task Valuer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<Valuer>(
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static Task Valuer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<Valuer>(
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static void GetHashCode_MissingMatchThrows()
    {
        new Valuer(Tools.Valuer.Options with { IncludeFrameworkHints = false })
            .Assert(v => v.GetHashCode(new object()))
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal static void GetHashCode_ValidHint(object data, int result, [Stub] ICompareHint hint)
    {
        hint.TryToGetHashCode(data, Arg.Any<IValuerChainer>())
            .SetupReturn(new HashCodeHintResult(result));

        new Valuer(Tools.Valuer.Options with { IncludeFrameworkHints = false, Hints = [hint] })
            .GetHashCode(data)
            .Assert()
            .Is(result);

        hint.Assert().Called();
    }

    [Fact]
    internal static void Compare_MissingMatchThrows()
    {
        new Valuer(Tools.Valuer.Options with { IncludeFrameworkHints = false })
            .Assert(v => v.Compare(new object(), new object()).ToList())
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal static void Compare_ReferenceNoDifferences(object data)
    {
        new Valuer(Tools.Valuer.Options with { IncludeFrameworkHints = false })
            .Compare(data, data)
            .Assert()
            .IsEmpty();
    }

    [Theory, RandomData]
    internal static void Compare_NullableWorks(int? item)
    {
        item.Assert().IsNot(null);
        item.Tools().Variant().Assert().IsNot(item);
        item.Tools().Copy().Assert().Is(item);

        int? none = null;
        none.Assert().IsNot(item);
        none.Assert().Is(none);
    }

    [Theory, RandomData]
    internal static void Equals_NoDifferencesTrue(
        object data1,
        object data2,
        [Stub] ICompareHint hint
    )
    {
        hint.TryToCompare(data1, data2, Arg.Any<IValuerChainer>())
            .SetupReturn(new DifferenceHintResult([]));

        new Valuer(Tools.Valuer.Options with { IncludeFrameworkHints = false, Hints = [hint] })
            .Equals(data1, data2)
            .Assert()
            .Is(true);
    }

    [Theory, RandomData]
    internal static void Equals_DifferencesFalse(
        object data1,
        object data2,
        [Stub] ICompareHint hint,
        IEnumerable<Difference> differences
    )
    {
        hint.TryToCompare(data1, data2, Arg.Any<IValuerChainer>())
            .SetupReturn(new(differences), Times.Once);

        new Valuer(Tools.Valuer.Options with { IncludeFrameworkHints = false, Hints = [hint] })
            .Equals(data1, data2)
            .Assert()
            .Is(false);

        hint.Assert().Called();
    }

    [Theory, RandomData]
    internal static void Compare_InfiniteLoopDetails(
        object item1,
        object item2,
        [Fake] ICompareHint hint
    )
    {
        hint.TryToCompare(item1, item2, Arg.Any<IValuerChainer>())
            .SetupReturn(
                Behavior<DifferenceHintResult>.Throw<InsufficientExecutionStackException>()
            );

        new Valuer(Tools.Valuer.Options with { IncludeFrameworkHints = false, Hints = [hint] })
            .Assert(v => v.Compare(item1, item2).ToList())
            .Throws<ToolException>()
            .With.Message.Assert()
            .Contains(GenericConverter.ExpandName(item1));
    }

    [Theory, RandomData]
    internal static void GetHashCode_InfiniteLoopDetails(object item, [Fake] ICompareHint hint)
    {
        hint.TryToGetHashCode(item, Arg.Any<IValuerChainer>())
            .SetupReturn(Behavior<HashCodeHintResult>.Throw<InsufficientExecutionStackException>());

        new Valuer(Tools.Valuer.Options with { IncludeFrameworkHints = false, Hints = [hint] })
            .Assert(v => v.GetHashCode(item))
            .Throws<ToolException>()
            .With.Message.Assert()
            .Contains(GenericConverter.ExpandName(item));
    }
}
