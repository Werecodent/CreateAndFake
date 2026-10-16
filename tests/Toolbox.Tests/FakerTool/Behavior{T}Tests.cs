using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.FakerTool;

namespace Werecodent.CreateAndFake.Tests.FakerTool;

public static class Behavior_T_Tests
{
    private static readonly TesterMod _Config = opt =>
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
            _Config
        );
    }

    [Fact]
    internal static Task Behavior_T_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(Behavior<>),
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static void Throw_DefaultBehaviorWorks()
    {
        Behavior<string>.Throw().Assert(x => x.Invoke([])).Throws<BehaviorDefaultThrowException>();
    }

    [Fact]
    internal static void Throw_BehaviorWorks()
    {
        Behavior<string>
            .Throw<InvalidOperationException>()
            .Assert(x => x.Invoke([]))
            .Throws<InvalidOperationException>();
    }
}
