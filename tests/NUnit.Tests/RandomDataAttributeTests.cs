using System.Reflection;
using CreateAndFake.DuplicatorTool;
using CreateAndFake.DuplicatorTool.Engine;
using CreateAndFake.FakerTool;
using NUnit.Framework.Internal;

namespace CreateAndFake.NUnit.Tests;

[TestFixture]
public static class RandomDataAttributeTests
{
    [RandomData]
    public static Task RandomDataAttribute_GuardsNulls([Stub] Test testStub)
    {
        return Tools.Tester.PreventsNullRefException(
            new RandomDataAttribute() { Trials = 3 },
            opt => opt with { InjectionValues = [3, GetGeneratableMethod(), testStub] }
        );
    }

    [RandomData]
    public static Task RandomDataAttribute_NoParameterMutation(
        [Stub] Test testStub,
        [Stub] CopyHint<MethodWrapper> copyStub
    )
    {
        copyStub
            .ToFake()
            .Setup(
                "Copy",
                [Arg.LambdaAny<MethodWrapper>(), Arg.LambdaAny<DuplicatorChainer>()],
                Behavior.Set<MethodWrapper, DuplicatorChainer, MethodWrapper>(
                    (w, _) => new MethodWrapper(w.TypeInfo.Type, w.MethodInfo)
                )
            );

        return Tools.Tester.PreventsParameterMutation(
            new RandomDataAttribute() { Trials = 3 },
            opt =>
                opt with
                {
                    InjectionValues = [3, GetGeneratableMethod(), testStub],
                    Duplicator = new Duplicator(
                        Tools.Duplicator.Options with
                        {
                            Hints = [copyStub],
                        }
                    ),
                }
        );
    }

    [RandomData]
    public static void GetData_UsesTrials([Stub] Test testStub)
    {
        MethodInfo method = GetGeneratableMethod();
        MethodWrapper wrapper = new(method.ReflectedType, method);
        new RandomDataAttribute() { Trials = 0 }
            .BuildFrom(wrapper, testStub)
            .Assert()
            .HasCount(0);
        new RandomDataAttribute() { Trials = 1 }
            .BuildFrom(wrapper, testStub)
            .Assert()
            .HasCount(1);
        new RandomDataAttribute() { Trials = 2 }
            .BuildFrom(wrapper, testStub)
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
