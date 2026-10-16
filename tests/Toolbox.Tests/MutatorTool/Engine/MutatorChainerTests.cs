using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.MutatorTool.Engine;

namespace Werecodent.CreateAndFake.Tests.MutatorTool.Engine;

public static class MutatorChainerTests
{
    [Fact]
    internal static Task MutatorChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<MutatorChainer>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(ToolException)] }
        );
    }

    [Fact]
    internal static Task MutatorChainer_PassthroughWithNoExceptions()
    {
        return Tools.Tester.PassthroughWithNoExceptionsAsync<MutatorChainer>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions = [typeof(InvalidCastException), typeof(ToolException)],
                }
        );
    }

    [Theory, RandomData]
    internal static void MutatorChainer_RecursionLimited([Stub] IMutatorEngine engine, string data)
    {
        engine
            .Modify(Arg.Any<string>(), Arg.Any<IMutatorChainer>())
            .SetupReturn(
                Behavior.Call<object, IMutatorChainer, bool>(
                    (_, chainer) => chainer.Modify(Tools.Randomizer.Create<string>())
                )
            );

        new MutatorChainer(
            Tools.Mutator.Options with
            {
                Valuer = Tools.Valuer.WithOptions(o => o with { IterationLimit = 2 }),
            },
            engine
        )
            .Assert(x => x.Modify(data))
            .Throws<EngineException>();
    }

    [Theory, RandomData]
    internal static void Modify_HandlesRecursionLoop([Stub] IMutatorEngine engine, object data)
    {
        engine
            .Modify(data, Arg.Any<IMutatorChainer>())
            .SetupReturn(
                Behavior.Call<object, IMutatorChainer, bool>((d, chainer) => chainer.Modify(d))
            );

        new MutatorChainer(Tools.Mutator.Options, engine).Modify(data).Assert().Is(false);
    }
}
