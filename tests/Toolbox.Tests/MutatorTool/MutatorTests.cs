using System.Reflection;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.MutatorTool;

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
}
