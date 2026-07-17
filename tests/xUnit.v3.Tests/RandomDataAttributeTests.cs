using System.Reflection;
using CreateAndFake.FakerTool;
using Xunit.Sdk;

namespace CreateAndFake.xUnit.v3.Tests;

public static class RandomDataAttributeTests
{
    [Fact]
    internal static async Task RandomDataAttribute_GuardsNulls()
    {
        await using DisposalTracker tracker = new();
        await Tools.Tester.PreventsNullRefExceptionAsync(
            new RandomDataAttribute() { Trials = 3 },
            TestContext.Current.CancellationToken,
            opt => opt with { InjectionValues = [3, GetGeneratableMethod(), tracker] }
        );
    }

    [Fact]
    internal static async Task RandomDataAttribute_NoParameterMutation()
    {
        await using DisposalTracker tracker = new();
        await Tools.Tester.PreventsParameterMutationAsync(
            new RandomDataAttribute() { Trials = 3 },
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    InjectionValues = [3, GetGeneratableMethod(), tracker],
                    IgnorableExceptions =
                    [
                        typeof(InvalidOperationException),
                        typeof(ArgumentException),
                    ],
                }
        );
    }

    [Fact(Timeout = 5000)]
    internal static async Task GetData_UsesTrials()
    {
        (await new RandomDataAttribute() { Trials = 0 }.GetData(GetGeneratableMethod(), null))
            .Assert()
            .HasCount(0);
        (await new RandomDataAttribute() { Trials = 1 }.GetData(GetGeneratableMethod(), null))
            .Assert()
            .HasCount(1);
        (await new RandomDataAttribute() { Trials = 2 }.GetData(GetGeneratableMethod(), null))
            .Assert()
            .HasCount(2);
    }

    [Theory, RandomData]
    internal static Task GetData_HandlesException([Fake] MethodInfo method)
    {
        method.IsGenericMethodDefinition.SetupReturn(Behavior<bool>.Throw(Times.Once));

        return new RandomDataAttribute()
            .GetData(method, null)
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
            .Also(() => method)
            .Called();
    }

    private static MethodInfo GetGeneratableMethod()
    {
        return Tools.Randomizer.Create<MethodInfo>(opt =>
            opt with
            {
                FinalCondition = m =>
                    m is MethodInfo info
                    && !info.IsGenericMethod
                    && !info.IsGenericMethodDefinition,
            }
        );
    }
}
