using CreateAndFake.DuplicatorTool;
using CreateAndFake.DuplicatorTool.Engine;
using CreateAndFake.FakerTool;

namespace CreateAndFake.Tests.DuplicatorTool;

public static class DuplicatorTests
{
    [Fact]
    internal static Task Duplicator_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<Duplicator>();
    }

    [Fact]
    internal static Task Duplicator_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<Duplicator>();
    }

    [Fact]
    internal static void Copy_MissingMatchThrows()
    {
        new Duplicator(Tools.Duplicator.Options with { IncludeDefaultHints = false })
            .Assert(d => d.Copy(new object()))
            .Throws<NotSupportedException>();
    }

    [Fact]
    internal static void Copy_NullWorks()
    {
        new Duplicator(Tools.Duplicator.Options with { IncludeDefaultHints = false })
            .Copy<object>(null)
            .Assert()
            .Is(null);
    }

    [Theory, RandomData]
    internal static void Copy_ValidHintWorks(object data, [Stub] CopyHint hint)
    {
        hint.TryCopy(data, Arg.Any<DuplicatorChainer>())
            .SetupReturn(new CopyHintResult(data), Times.Once);

        new Duplicator(
            Tools.Duplicator.Options with
            {
                IncludeDefaultHints = false,
                Hints = [hint],
            }
        )
            .Copy(data)
            .Assert()
            .Is(data);

        hint.Assert().Called(Times.Once);
    }

    [Theory, RandomData]
    internal static void Copy_InfiniteLoopDetails(object instance, [Stub] CopyHint hint)
    {
        hint.ToFake()
            .Setup(
                m => m.TryCopy(instance, Arg.Any<DuplicatorChainer>()),
                Behavior.Throw<InsufficientExecutionStackException>(Times.Once)
            );

        new Duplicator(
            Tools.Duplicator.Options with
            {
                IncludeDefaultHints = false,
                Hints = [hint],
            }
        )
            .Assert(d => d.Copy(instance))
            .Throws<InsufficientExecutionStackException>()
            .Message.Assert()
            .Contains(instance.GetType().Name);
    }
}
