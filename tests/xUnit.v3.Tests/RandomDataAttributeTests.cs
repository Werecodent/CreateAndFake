using System.Reflection;
using Werecodent.CreateAndFake.FakerTool;
using Xunit.Sdk;

namespace Werecodent.CreateAndFake.xUnit.v3.Tests;

#pragma warning disable RCS1261 // Not available in all .NET versions.

public static class RandomDataAttributeTests
{
    [Fact]
    internal static async Task RandomDataAttribute_GuardsNulls()
    {
        await using DisposalTracker tracker = new();
        await Tools.Tester.PreventsNullRefExceptionAsync(
            new RandomDataAttribute() { Trials = 3 },
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    InjectionValues = [3, GetGeneratableMethod(), tracker],
                    OnlyDeclaredMethods = true,
                }
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
                    MethodsToIgnore = [nameof(RandomDataAttribute.GetData)],
                    OnlyDeclaredMethods = true,
                }
        );
    }

    [Fact]
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
    internal static async Task GetData_HandlesExceptionAndDisposal([Fake] MethodInfo method)
    {
        method.IsGenericMethodDefinition.SetupReturn(Behavior<bool>.Throw(Times.Once));

        using StringWriter writer = new();

        TextWriter errorConsole = Console.Error;
        Console.SetError(writer);
        try
        {
            _ = await new RandomDataAttribute()
                .GetData(method, null)
                .Assert()
                .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
                .Also(() => method)
                .Called();

            await SafeDisposer
                .TryTracking(
                    nameof(GetData_HandlesExceptionAndDisposal),
                    [
                        Tools.Faker.Mock<IDisposable>().Dummy,
                        Tools.Faker.Mock<IAsyncDisposable>().Dummy,
                    ]
                )
                .DisposeAsync();
        }
        finally
        {
            Console.SetError(errorConsole);
        }
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

#pragma warning restore
