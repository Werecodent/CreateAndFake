using System.Reflection;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.FakerTool;
using CreateAndFake.RunnerTool;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.RunnerTool;

public static class MethodCallWrapperTests
{
    private static readonly TesterMod config = opt =>
        opt with
        {
            IgnorableExceptions =
            [
                typeof(KeyNotFoundException),
                typeof(ArgumentOutOfRangeException),
                typeof(TargetException),
                typeof(ToolException),
                typeof(ArgumentException),
                typeof(TargetParameterCountException),
                typeof(InvalidOperationException),
                typeof(FormatException),
            ],
        };

    [Fact]
    internal static Task MethodCallWrapper_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<MethodCallWrapper>(
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static Task MethodCallWrapper_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<MethodCallWrapper>(
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static void MethodCallWrapper_CanRandomize()
    {
        Tools.Randomizer.Create<MethodCallWrapper>().Assert().IsNot(null);
    }

    [Theory, RandomData]
    internal static void MethodCallWrapper_WorksInFake(
        [Stub] IRunner runner,
        MethodCallWrapper wrapper,
        MethodBase method
    )
    {
        runner
            .CreateFor(Arg.Any<MethodBase>(), Arg.Any<CancellationToken>(), Arg.Any<object[]>())
            .SetupReturn(wrapper);
        runner.CreateFor(method, TestContext.Current.CancellationToken).Assert().Is(wrapper);
    }

    [Theory, RandomData]
    internal static void ModifyArg_ThrowsWithUnknownParameter(
        MethodCallWrapper method,
        string parameter,
        object value
    )
    {
        method.Assert(m => m.ModifyArg(parameter, value)).Throws<KeyNotFoundException>();
    }

    [Theory, RandomData]
    internal static void ModifyArg_CanMutate(DataSample sample)
    {
        MethodCallWrapper wrapper = Tools.Runner.CreateFor(
            typeof(DataHolderSample).GetMethod(nameof(DataHolderSample.HasNested)),
            TestContext.Current.CancellationToken
        );

        wrapper.ModifyArg("value", sample);
        wrapper.Args.Assert().Contains(sample);
    }

    [Theory, RandomData]
    internal static void InvokeOn_UsesArgs(DataSample sample)
    {
        MethodCallWrapper wrapper = Tools.Runner.CreateFor(
            typeof(DataSample).GetMethod(nameof(Equals)),
            TestContext.Current.CancellationToken
        );

        wrapper.InvokeOn(sample).Assert().Is(false);
        wrapper.ModifyArg("obj", sample);
        wrapper.InvokeOn(sample).Assert().Is(true);
    }
}
