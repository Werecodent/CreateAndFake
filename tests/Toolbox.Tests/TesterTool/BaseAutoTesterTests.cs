using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.Tests.TesterTool;

public static class BaseAutoTesterTests
{
    private sealed class InjectedAutoTester(IToolSet tools)
        : AutoTester(typeof(ValueEquatableSample))
    {
        protected override IToolSet AutoTools { get; } = tools;
    }

    [Theory, RandomData]
    internal static async Task BaseAutoTester_WrapsExceptions([Stub] IToolSet tools)
    {
        ITester tester = Tools.Faker.Mock<ITester>().Dummy;
        IAsserter asserter = Tools.Faker.Mock<IAsserter>().Dummy;

        tools.Tester.SetupReturn(tester);
        tools.Asserter.SetupReturn(asserter);

        InjectedAutoTester testInstance = new(tools);
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
}
