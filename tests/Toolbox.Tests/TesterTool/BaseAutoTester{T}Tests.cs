using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.Tests.TesterTool;

public static class BaseAutoTester_T_Tests
{
    private sealed class NullInstanceTester(IToolSet tools) : AutoTester<ValueEquatableSample>
    {
        protected override IToolSet AutoTools { get; } = tools;

        protected override ValueEquatableSample TestInstance { get; } = null;
    }

    [Theory, RandomData]
    internal static async Task BaseAutoTester_T_WrapsExceptions([Stub] IToolSet tools)
    {
        ITester tester = Tools.Faker.Mock<ITester>().Dummy;
        IAsserter asserter = Tools.Faker.Mock<IAsserter>().Dummy;

        tools.Tester.SetupReturn(tester);
        tools.Asserter.SetupReturn(asserter);

        NullInstanceTester testInstance = new(tools);
        testInstance.Assert(x => x.Tester_Debug_ToString()).Throws<AutoTesterFailureException>();
        await testInstance
            .Tester_GuardsNulls()
            .Assert()
            .ThrowsAsync<AutoTesterFailureException>(TestContext.Current.CancellationToken);
        await testInstance
            .Tester_NoParameterMutation()
            .Assert()
            .ThrowsAsync<AutoTesterFailureException>(TestContext.Current.CancellationToken);
        await testInstance
            .Tester_ValueEqualityOverrideCheck()
            .Assert()
            .ThrowsAsync<AutoTesterFailureException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task TestInstance_AltersBehaviorWhenNull(
        [Stub] IToolSet tools,
        [Stub] ITester tester
    )
    {
        tools.Tester.SetupReturn(tester);
        tester
            .PreventsNullRefExceptionAsync<AnyGeneric>(
                Arg.Any<CancellationToken>(),
                Arg.Any<TesterMod>()
            )
            .SetupReturn(Task.CompletedTask);
        tester
            .PreventsParameterMutationAsync<AnyGeneric>(
                Arg.Any<CancellationToken>(),
                Arg.Any<TesterMod>()
            )
            .SetupReturn(Task.CompletedTask);
        tester.Tools().ToFake().ThrowByDefault = true;

        NullInstanceTester testInstance = new(tools);
        await testInstance.Tester_GuardsNulls();
        await testInstance.Tester_NoParameterMutation();

        tester.Assert().Called();
    }
}
