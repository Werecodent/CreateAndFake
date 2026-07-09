using System.Collections.Frozen;
using System.Reflection;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Types;
using CreateAndFake.FakerTool;

namespace CreateAndFake.Tests.FakerTool;

public static class BehaviorTests
{
    private static readonly TesterMod _Config = opt =>
        opt with
        {
            MethodsToIgnore = FrozenSet.ToFrozenSet(["Throw"]),
            IgnorableExceptions =
            [
                typeof(ArgumentException),
                typeof(ArgumentNullException),
                typeof(NotImplementedException),
                typeof(TargetParameterCountException),
            ],
        };

    [Fact]
    internal static Task Behavior_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(Behavior),
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task Behavior_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(Behavior),
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static void Call_BehaviorWorks()
    {
        foreach (
            MethodInfo info in typeof(Behavior)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(m => m.Name == nameof(Behavior.Call))
        )
        {
            Type type = info.GetParameters()[0].ParameterType;

            Type[] generics =
                GenericConverter
                    .AsGenericBase(type)
                    ?.GetGenericArguments()
                    .Select(_ => typeof(string))
                    .ToArray()
                ?? Type.EmptyTypes;

            MethodInfo caller = (generics.Length != 0) ? info.MakeGenericMethod(generics) : info;

            Type setupType = caller.GetParameters()[0].ParameterType;

            Type[] args = type.Name.StartsWith("Func", StringComparison.Ordinal)
                ? [.. generics.Skip(1)]
                : generics;

            Behavior noTimes = (Behavior)
                caller.Invoke(null, [Tools.Randomizer.Create(setupType), null]);

            noTimes.HasExpectedCalls().Assert().Is(false);
            noTimes.Invoke([.. args.Select(a => Tools.Randomizer.Create(a))]);
            noTimes.HasExpectedCalls().Assert().Is(true);

            Behavior withTimes = (Behavior)
                caller.Invoke(null, [Tools.Randomizer.Create(setupType), Times.Never]);

            withTimes.HasExpectedCalls().Assert().Is(true);
            withTimes.Invoke([.. args.Select(a => Tools.Randomizer.Create(a))]);
            withTimes.HasExpectedCalls().Assert().Is(false);
        }
    }

    [Fact]
    internal static void None_BehaviorWorks()
    {
        Behavior.None().Invoke([]).Assert().Is(default);
    }

    [Fact]
    internal static void Throw_DefaultBehaviorWorks()
    {
        Behavior.Throw().Assert(x => x.Invoke([])).Throws<BehaviorDefaultThrowException>();
    }

    [Fact]
    internal static void Null_BehaviorWorks()
    {
        Behavior.Null<string>().Invoke([]).Assert().IsNull();
    }

    [Fact]
    internal static void Default_BehaviorWorks()
    {
        Behavior.Default<int>().Invoke([]).Assert().Is(default(int));
    }

    [Fact]
    internal static void Throw_BehaviorWorks()
    {
        Behavior
            .Throw<InvalidOperationException>()
            .Assert(x => x.Invoke([]))
            .Throws<InvalidOperationException>();
    }

    [Theory, RandomData]
    internal static void Returns_BehaviorWorks(int value)
    {
        Behavior.Returns(value).Invoke([]).Assert().Is(value);
    }

    [Fact]
    internal static void Series_BehaviorWorks()
    {
        Behavior behavior = Behavior.Series(true, false, true);
        behavior.Invoke([]).Assert().Is(true);
        behavior.Invoke([]).Assert().Is(false);
        behavior.Invoke([]).Assert().Is(true);
        behavior.Invoke([]).Assert().Is(false);
        behavior.Invoke([]).Assert().Is(false);
    }

    [Theory, RandomData]
    internal static void Series_NullCountsAsNone(string value)
    {
        Behavior behavior = Behavior.Series(value, null);
        behavior.Invoke([]).Assert().Is(value);
        behavior.Invoke([]).Assert().IsNull();
        behavior.Invoke([]).Assert().IsNull();
    }

    [Theory, RandomData]
    internal static void ToExpectedCalls_MatchesTimes(Times times)
    {
        Behavior.None().ToExpectedCalls().Assert().Is(Times.AtLeast(1).ToString());
        Behavior.None(times).ToExpectedCalls().Assert().Is(times.ToString());
    }

    [Fact]
    internal static void Invoke_ThrowsWithWrongArgs()
    {
        Behavior
            .Call((int _) => { })
            .Assert(x => x.Invoke([]))
            .Throws<TargetParameterCountException>();
    }
}
