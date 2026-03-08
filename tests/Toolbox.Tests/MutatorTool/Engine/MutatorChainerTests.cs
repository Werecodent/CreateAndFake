using CreateAndFake.FakerTool;
using CreateAndFake.MutatorTool.Engine;

namespace CreateAndFake.Tests.MutatorTool.Engine;

public static class MutatorChainerTests
{
    [Fact]
    internal static Task MutatorChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<MutatorChainer>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task MutatorChainer_PassthroughWithNoExceptions()
    {
        return Tools.Tester.PassthroughWithNoExceptionsAsync<MutatorChainer>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(InvalidCastException)] }
        );
    }

    [Theory, RandomData]
    internal static void Modify_HandlesRecursionLoop([Stub] IMutatorEngine engine, object data)
    {
        engine
            .Modify(data, Arg.Any<IMutatorChainer>())
            .SetupCall(
                Behavior.Set<object, IMutatorChainer, bool>((d, chainer) => chainer.Modify(d))
            );

        new MutatorChainer(Tools.Mutator.Options, engine).Modify(data).Assert().Is(false);
    }
}
