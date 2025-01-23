using System.Reflection;

namespace CreateAndFake.MSTestTests;

[TestClass]
public class RandomDataAttributeTests
{
    [TestMethod]
    public void RandomDataAttribute_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<RandomDataAttribute>();
    }

    [TestMethod]
    public void RandomDataAttribute_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<RandomDataAttribute>(opt => opt with { InjectionValues = [3] });
    }

    [TestMethod]
    public void GetData_UsesTrials()
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
