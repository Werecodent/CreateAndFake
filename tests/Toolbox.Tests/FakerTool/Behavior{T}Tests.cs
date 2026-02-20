using CreateAndFake.FakerTool;

namespace CreateAndFake.Tests.FakerTool;

public static class Behavior_T_Tests
{
    private static readonly TesterMod config = opt =>
        opt with
        {
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
        return Tools.Tester.PreventsNullRefException(
            typeof(Behavior<>),
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static Task Behavior_T_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(
            typeof(Behavior<>),
            TestContext.Current.CancellationToken,
            opt => config(opt) with { MethodsToIgnore = ["Throw"] }
        );
    }

    [Fact]
    internal static void Error_BehaviorWorks()
    {
        Behavior<string>.Error().Assert(b => b.Invoke([])).Throws<NotImplementedException>();
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
