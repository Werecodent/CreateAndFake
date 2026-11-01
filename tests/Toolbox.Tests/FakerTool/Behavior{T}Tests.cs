using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.FakerTool;

namespace Werecodent.CreateAndFake.Tests.FakerTool;

public sealed class Behavior_T_Tests() : AutoTester(typeof(Behavior<>))
{
    protected override TesterMod OptionConfiguration { get; } =
        opt =>
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
