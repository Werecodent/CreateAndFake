using System.Reflection;
using CreateAndFake.FakerTool;
using CreateAndFake.RunnerTool;
using CreateAndFake.Samples.OldSamples;

namespace CreateAndFake.Tests.RunnerTool;

public static class MethodCallWrapperTests
{
    [Fact]
    internal static Task MethodCallWrapper_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<MethodCallWrapper>();
    }

    [Fact]
    internal static Task MethodCallWrapper_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<MethodCallWrapper>();
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
        runner.CreateFor(Arg.Any<MethodBase>(), Arg.Any<object[]>()).SetupReturn(wrapper);
        runner.CreateFor(method).Assert().Is(wrapper);
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
            typeof(DataHolderSample).GetMethod(nameof(DataHolderSample.HasNested))
        );

        wrapper.ModifyArg("value", sample);
        wrapper.Args.Assert().Contains(sample);
    }

    [Theory, RandomData]
    internal static void InvokeOn_UsesArgs(DataSample sample)
    {
        MethodCallWrapper wrapper = Tools.Runner.CreateFor(
            typeof(DataSample).GetMethod(nameof(Equals))
        );

        wrapper.InvokeOn(sample).Assert().Is(false);
        wrapper.ModifyArg("obj", sample);
        wrapper.InvokeOn(sample).Assert().Is(true);
    }
}
