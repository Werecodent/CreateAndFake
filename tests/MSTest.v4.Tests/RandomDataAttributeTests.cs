using System.Reflection;

namespace CreateAndFake.MSTest.v4.Tests;

[TestClass]
public class RandomDataAttributeTests
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    public Task RandomDataAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(
            new RandomDataAttribute() { Trials = 3 },
            TestContext.CancellationToken,
            opt => opt with { InjectionValues = [3, GetGeneratableMethod()] }
        );
    }

    [TestMethod]
    public Task RandomDataAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(
            new RandomDataAttribute() { Trials = 3 },
            TestContext.CancellationToken,
            opt => opt with { InjectionValues = [3, GetGeneratableMethod()] }
        );
    }

    [TestMethod]
    public void GetData_UsesTrials()
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
