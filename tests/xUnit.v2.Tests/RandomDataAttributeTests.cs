using System.Reflection;
using Werecodent.CreateAndFake.FakerTool;

namespace Werecodent.CreateAndFake.xUnit.v2.Tests;

public static class RandomDataAttributeTests
{
    [Fact]
    internal static Task RandomDataAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            new RandomDataAttribute() { Trials = 3 },
            CancellationToken.None,
            opt => opt with { InjectionValues = [3, GetGeneratableMethod()] }
        );
    }

    [Fact]
    internal static Task RandomDataAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            new RandomDataAttribute() { Trials = 3 },
            CancellationToken.None,
            opt => opt with { InjectionValues = [3, GetGeneratableMethod()] }
        );
    }

    [Fact]
    internal static void GetData_UsesTrials()
    {
        new RandomDataAttribute() { Trials = 0 }
            .GetData(GetGeneratableMethod())
            .Assert()
            .HasCount(0);
        new RandomDataAttribute() { Trials = 1 }
            .GetData(GetGeneratableMethod())
            .Assert()
            .HasCount(1);
        new RandomDataAttribute() { Trials = 2 }
            .GetData(GetGeneratableMethod())
            .Assert()
            .HasCount(2);
    }

    [Theory, RandomData]
    internal static void GetData_HandlesException([Fake] MethodInfo method)
    {
        method.IsGenericMethodDefinition.SetupReturn(Behavior<bool>.Throw(Times.Once));

        new RandomDataAttribute().GetData(method).Assert().IsEmpty().Also(method).Called();
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
