using System.Reflection;
using Xunit.Sdk;

namespace CreateAndFake.xUnit.v3.Tests;

public static class RandomDataAttributeTests
{
    [Fact]
    internal static async Task RandomDataAttribute_GuardsNulls()
    {
        await using DisposalTracker tracker = new();
        Tools.Tester.PreventsNullRefException(new RandomDataAttribute() { Trials = 3 }, opt => opt with
        {
            InjectionValues = [3, GetGeneratableMethod(), tracker]
        });
    }

    [Fact]
    internal static async Task RandomDataAttribute_NoParameterMutation()
    {
        await using DisposalTracker tracker = new();
        Tools.Tester.PreventsParameterMutation(new RandomDataAttribute() { Trials = 3 }, opt => opt with
        {
            InjectionValues = [3, GetGeneratableMethod(), tracker]
        });
    }

    [Fact]
    internal static async Task GetData_UsesTrials()
    {
        (await new RandomDataAttribute() { Trials = 0 }.GetData(GetGeneratableMethod(), null)).Assert().HasCount(0);
        (await new RandomDataAttribute() { Trials = 1 }.GetData(GetGeneratableMethod(), null)).Assert().HasCount(1);
        (await new RandomDataAttribute() { Trials = 2 }.GetData(GetGeneratableMethod(), null)).Assert().HasCount(2);
    }

    private static MethodInfo GetGeneratableMethod()
    {
        return Tools.Randomizer.Create<MethodInfo>(opt => opt with
        {
            FinalCondition = m => m is MethodInfo info && !info.IsGenericMethod && !info.IsGenericMethodDefinition
        });
    }
}
