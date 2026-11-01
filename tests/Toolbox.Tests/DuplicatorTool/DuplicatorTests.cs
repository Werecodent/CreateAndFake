using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.DuplicatorTool;
using Werecodent.CreateAndFake.DuplicatorTool.Engine;
using Werecodent.CreateAndFake.FakerTool;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool;

public sealed class DuplicatorTests : AutoTester<Duplicator>
{
    protected override TesterMod OptionConfiguration { get; } =
        opt => opt with { IgnorableExceptions = [typeof(ToolException)] };

    [Fact]
    internal static void Copy_MissingMatchThrows()
    {
        new Duplicator(Tools.Duplicator.Options with { IncludeFrameworkHints = false })
            .Assert(x => x.Copy(new object()))
            .Throws<ToolException>()
            .With(e => e.InnerException.GetType())
            .Is(typeof(UnsupportedException));
    }

    [Fact]
    internal static void Copy_NullWorks()
    {
        new Duplicator(Tools.Duplicator.Options with { IncludeFrameworkHints = false })
            .Copy<object>(null)
            .Assert()
            .IsNull();
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
        hint.Tools()
            .ToFake()
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
            .Assert(x => x.Copy(instance))
            .Throws<ToolException>()
            .With(e => e.Message)
            .Contains(GenericConverter.ExpandName(instance));
    }
}
