using System.Reflection;
using CreateAndFake.DuplicatorTool;
using CreateAndFake.DuplicatorTool.Engine;
using CreateAndFake.FakerTool;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace CreateAndFake.NUnit.v3.Tests;

[TestFixture]
public static class RandomDataAttributeTests
{
    [RandomData]
    public static Task RandomDataAttribute_GuardsNulls([Stub] Test testStub)
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            new RandomDataAttribute() { Trials = 3 },
            TestContext.CurrentContext.CancellationToken,
            opt => opt with { InjectionValues = [3, GetGeneratableMethod(), testStub] }
        );
    }

    [RandomData]
    public static Task RandomDataAttribute_NoParameterMutation(
        [Stub] Test testStub,
        [Stub] ICopyHint copyStub
    )
    {
        copyStub
            .TryCopy(Arg.Any<MethodWrapper>(), Arg.Any<IDuplicatorChainer>())
            .SetupReturn(
                Behavior.Call<MethodWrapper, IDuplicatorChainer, CopyHintResult>(
                    (w, _) =>
                        (w != null)
                            ? new(new MethodWrapper(w.TypeInfo.Type, w.MethodInfo))
                            : CopyHintResult.None
                )
            );

        return Tools.Tester.PreventsParameterMutationAsync(
            new RandomDataAttribute() { Trials = 3 },
            TestContext.CurrentContext.CancellationToken,
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

    [RandomData]
    public static void GetData_HandlesException([Stub] Test suite, [Fake] IMethodInfo method)
    {
        method.MethodInfo.SetupReturn(Behavior<MethodInfo>.Throw(Times.Once));

        new RandomDataAttribute().BuildFrom(method, suite).Assert().IsEmpty().Also(method).Called();
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
