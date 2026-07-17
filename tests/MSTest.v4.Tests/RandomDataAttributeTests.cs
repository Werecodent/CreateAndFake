using System.Reflection;
using CreateAndFake.FakerTool;

namespace CreateAndFake.MSTest.v4.Tests;

[TestClass]
public class RandomDataAttributeTests
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    public Task RandomDataAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            new RandomDataAttribute() { Trials = 3 },
            TestContext.CancellationToken,
            opt => opt with { InjectionValues = [3, GetGeneratableMethod()] }
        );
    }

    [TestMethod]
    public Task RandomDataAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
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

    [TestMethod, RandomData]
    public void GetData_HandlesException([Fake] MethodInfo method)
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
