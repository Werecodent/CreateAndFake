using System.Reflection;

namespace CreateAndFake.xUnit.v3Tests;

public static class RandomDataAttributeTests
{
    [Fact]
    internal static void RandomDataAttribute_GuardsNulls()
    {
        //Tools.Tester.PreventsNullRefException<RandomDataAttribute>();
    }

    [Fact]
    internal static void RandomDataAttribute_NoParameterMutation()
    {
        //Tools.Tester.PreventsParameterMutation<RandomDataAttribute>(opt => opt with { InjectionValues = [3] });
    }

    [Fact]
    internal static async Task GetData_UsesTrials()
    {
        MethodInfo method = Tools.Randomizer.Create<MethodInfo>(opt => opt with
        {
            FinalCondition = m => m is MethodInfo info && !info.IsGenericMethod && !info.IsGenericMethodDefinition
        });

        Tools.Asserter.HasCount(0, await new RandomDataAttribute() { Trials = 0 }.GetData(method, null));
        Tools.Asserter.HasCount(1, await new RandomDataAttribute() { Trials = 1 }.GetData(method, null));
        Tools.Asserter.HasCount(2, await new RandomDataAttribute() { Trials = 2 }.GetData(method, null));
    }
}
