using CreateAndFake.Design.Content;
using CreateAndFake.Design.Tooling;
using CreateAndFake.DuplicatorTool;
using CreateAndFake.DuplicatorTool.Engine;
using CreateAndFake.FakerTool;

namespace CreateAndFake.Tests.DuplicatorTool;

public static class DuplicatorTests
{
    private static readonly TesterMod config = opt =>
        opt with
        {
            IgnorableExceptions = [typeof(ToolException)],
        };

    [Fact]
    internal static Task Duplicator_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<Duplicator>(
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static Task Duplicator_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<Duplicator>(
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static void Copy_MissingMatchThrows()
    {
        new Duplicator(Tools.Duplicator.Options with { IncludeFrameworkHints = false })
            .Assert(d => d.Copy(new object()))
            .Throws<ToolException>()
            .InnerException.GetType()
            .Assert()
            .Is(typeof(NotSupportedException));
    }

    [Fact]
    internal static void Copy_NullWorks()
    {
        new Duplicator(Tools.Duplicator.Options with { IncludeFrameworkHints = false })
            .Copy<object>(null)
            .Assert()
            .Is(null);
    }

    [Theory, RandomData]
    internal static void Copy_ValidHintWorks(object data, [Stub] CopyHint hint)
    {
        hint.TryCopy(data, Arg.Any<IDuplicatorChainer>())
            .SetupReturn(new CopyHintResult(data), Times.Once);

        new Duplicator(
            Tools.Duplicator.Options with
            {
                IncludeFrameworkHints = false,
                Hints = [hint],
            }
        )
            .Copy(data)
            .Assert()
            .Is(data);

        hint.Assert().Called();
    }

    [Theory, RandomData]
    internal static void Copy_InfiniteLoopDetails(object instance, [Stub] CopyHint hint)
    {
        hint.ToFake()
            .Setup(
                m => m.TryCopy(instance, Arg.Any<IDuplicatorChainer>()),
                Behavior.Throw<InsufficientExecutionStackException>(Times.Once)
            );

        new Duplicator(
            Tools.Duplicator.Options with
            {
                IncludeFrameworkHints = false,
                Hints = [hint],
            }
        )
            .Assert(d => d.Copy(instance))
            .Throws<ToolException>()
            .Message.Assert()
            .Contains(TypeDescriber.ExpandedName(instance.GetType()));
    }
}
