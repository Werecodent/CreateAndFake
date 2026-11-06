using System.Reflection;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.MutatorTool;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.Tests.MutatorTool;

public sealed class MutatorTests : AutoTester<Mutator>
{
    protected override TesterMod OptionConfiguration { get; } =
        opt => opt with { IgnorableExceptions = [typeof(ToolException)] };

    public override Task Tester_NoParameterMutation()
    {
        return Task.CompletedTask;
    }

    [Fact]
    internal static Task Mutator_PassthroughWithNoExceptions()
    {
        return Tools.Tester.PassthroughWithNoExceptionsAsync<Mutator>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(TargetInvocationException)] }
        );
    }

    [Theory, RandomData]
    internal static void Modify_UsesMaxAttemptsForModify([Fake] IValuer valuer, DataSample sample)
    {
        MutatorOptions mod(MutatorOptions opt)
        {
            return opt with
            {
                Valuer = valuer,
                MaxAttemptsForModify = 2,
                VerifyModifyViaValueHashes = true,
                NestedOptions = opt,
            };
        }

        Tools.Mutator.WithOptions(mod).Modify(sample).Assert().Is(true);
        Tools.Mutator.Modify(sample, mod).Assert().Is(true);

        valuer.GetHashCode(sample, Arg.Any<ValuerMod>()).SetupReturn(0);

        Tools.Mutator.WithOptions(mod).Modify(sample).Assert().Is(false);
        Tools.Mutator.Modify(sample, mod).Assert().Is(false);

        valuer.GetHashCode(sample, Arg.Any<ValuerMod>()).SetupReturn(Behavior.Series(0, 1, 0, 1));

        Tools.Mutator.WithOptions(mod).Modify(sample).Assert().Is(true);
        Tools.Mutator.Modify(sample, mod).Assert().Is(true);
    }

    [Theory, RandomData]
    internal static void Modify_CanDisableVerifyModifyViaValueHashes(
        [Stub] IValuer valuer,
        DataSample sample
    )
    {
        MutatorOptions mod(MutatorOptions opt)
        {
            return opt with
            {
                Valuer = valuer,
                MaxAttemptsForModify = 2,
                VerifyModifyViaValueHashes = false,
                NestedOptions = opt,
            };
        }

        Tools.Mutator.Modify(sample, mod).Assert().Is(true);
        Tools.Mutator.WithOptions(mod).Modify(sample).Assert().Is(true);
    }
}
