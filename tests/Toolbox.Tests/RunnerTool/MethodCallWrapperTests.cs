using System.Collections.Frozen;
using System.Collections.Specialized;
using System.Reflection;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.RunnerTool;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.RunnerTool;

public static class MethodCallWrapperTests
{
    [Theory, RandomData]
    internal static void Debug_MethodCallWrapper_ToString(MethodCallWrapper sample)
    {
        sample.ToString().Assert().Debug();
    }

    [Fact]
    internal static Task MethodCallWrapper_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<MethodCallWrapper>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions =
                    [
                        typeof(KeyNotFoundException),
                        typeof(ArgumentOutOfRangeException),
                    ],
                    MethodsToIgnore = opt
                        .MethodsToIgnore.Concat([nameof(MethodCallWrapper.InvokeOn)])
                        .ToFrozenSet(),
                }
        );
    }

    [Fact]
    internal static void MethodCallWrapper_CanRandomize()
    {
        Tools.Randomizer.Create<MethodCallWrapper>().Assert().IsNotNull();
    }

    [Theory, RandomData]
    internal static void MethodCallWrapper_WorksInFake(
        [Stub] IRunner runner,
        MethodCallWrapper wrapper,
        MethodBase method
    )
    {
        runner.CreateFor(Arg.Any<MethodBase>(), Arg.Any<CancellationToken>()).SetupReturn(wrapper);
        runner.CreateFor(method, TestContext.Current.CancellationToken).Assert().Is(wrapper);
    }

    [Theory, RandomData]
    internal static void ModifyArg_ThrowsWithUnknownParameter(
        MethodCallWrapper method,
        string parameter,
        object value
    )
    {
        method.Assert(x => x.ModifyArg(parameter, value)).Throws<KeyNotFoundException>();
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

    [Theory, RandomData]
    internal static Task CompareAsync_NoDifferencesWithCopy(
        MethodBase originalMethod,
        [Copy] MethodBase cloneMethod,
        OrderedDictionary originalArgs,
        [Copy] OrderedDictionary cloneArgs
    )
    {
        MethodCallWrapper original = new(originalMethod, originalArgs);
        MethodCallWrapper clone = new(cloneMethod, cloneArgs);

        return original
            .CompareAsync(clone, Tools.Valuer, TestContext.Current.CancellationToken)
            .Assert()
            .HasCountAsync(0, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static Task CompareAsync_DifferencesWithVariant(
        MethodBase originalMethod,
        MethodBase variantMethod,
        OrderedDictionary originalArgs,
        OrderedDictionary variantArgs
    )
    {
        MethodCallWrapper original = new(originalMethod, originalArgs);
        MethodCallWrapper variant = new(variantMethod, variantArgs);

        return original
            .CompareAsync(variant, Tools.Valuer, TestContext.Current.CancellationToken)
            .Assert()
            .HasCountMoreThanAsync(0, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static Task CompareAsync_DifferenceWithDifferentType(
        MethodCallWrapper wrapper,
        DataSample sample
    )
    {
        return wrapper
            .CompareAsync(sample, Tools.Valuer, TestContext.Current.CancellationToken)
            .Assert()
            .HasCountAsync(1, TestContext.Current.CancellationToken);
    }
}
