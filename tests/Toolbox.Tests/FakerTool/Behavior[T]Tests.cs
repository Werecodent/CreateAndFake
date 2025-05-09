using CreateAndFake.FakerTool;

namespace CreateAndFake.Tests.FakerTool;

public static class Behavior_T_Tests
{
    [Fact]
    internal static Task Behavior_T_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(typeof(Behavior<>));
    }

    [Fact]
    internal static Task Behavior_T_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(typeof(Behavior<>));
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
