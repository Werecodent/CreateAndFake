using System.Reflection;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.MSTest.v3.Tests;

[TestClass]
public sealed class RandomDataAttributeTests : AutoTester<RandomDataAttribute>
{
    protected override RandomDataAttribute TestInstance { get; } = new() { Trials = 3 };

    protected override Func<TesterOptions, TesterOptions> OptionConfiguration { get; } =
        opt => opt with { InjectionValues = [3, GetGeneratableMethod()] };

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

    [TestMethod, RandomData, DoNotParallelize]
    public void GetData_HandlesException([Fake] MethodInfo method)
    {
        method.IsGenericMethodDefinition.SetupReturn(Behavior<bool>.Throw(Times.Once));

        using StringWriter writer = new();

        TextWriter errorConsole = Console.Error;
        Console.SetError(writer);
        try
        {
            new RandomDataAttribute().GetData(method).Assert().IsEmpty().Also(method).Called();
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
