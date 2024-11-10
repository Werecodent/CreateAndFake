using System.Reflection;

namespace CreateAndFakeTests.Attributes;

/// <summary>Verifies behavior.</summary>
public static class RandomDataAttributeTests
{
    [Fact]
    internal static void TypeExtensions_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<RandomDataAttribute>();
    }

    [Fact]
    internal static void GetData_UsesTrials()
    {
        MethodInfo method = Tools.Randomizer.Create<MethodInfo>(opt => opt with
        {
            FinalCondition = m => m is MethodInfo info && !info.IsGenericMethod && !info.IsGenericMethodDefinition
        });

        Tools.Asserter.HasCount(0, new RandomDataAttribute() { Trials = 0 }.GetData(method));
        Tools.Asserter.HasCount(1, new RandomDataAttribute() { Trials = 1 }.GetData(method));
        Tools.Asserter.HasCount(2, new RandomDataAttribute() { Trials = 2 }.GetData(method));
    }
}
