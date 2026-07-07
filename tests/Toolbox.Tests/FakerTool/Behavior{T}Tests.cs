using CreateAndFake.Design.Exceptions;
using CreateAndFake.FakerTool;

namespace CreateAndFake.Tests.FakerTool;

public static class Behavior_T_Tests
{
    private static readonly TesterMod config = opt =>
        opt with
        {
            MethodsToIgnore = ["Throw"],
            IgnorableExceptions =
            [
                typeof(MemberAccessException),
                typeof(InvalidOperationException),
                typeof(NotImplementedException),
            ],
        };

    [Fact]
    internal static Task Behavior_T_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(Behavior<>),
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static Task Behavior_T_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(Behavior<>),
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static void Throw_DefaultBehaviorWorks()
    {
        Behavior<string>.Throw().Assert(b => b.Invoke([])).Throws<BehaviorDefaultThrowException>();
    }

    [Fact]
    internal static void Throw_BehaviorWorks()
    {
        Behavior<string>
            .Throw<InvalidOperationException>()
            .Assert(b => b.Invoke([]))
            .Throws<InvalidOperationException>();
    }
}
