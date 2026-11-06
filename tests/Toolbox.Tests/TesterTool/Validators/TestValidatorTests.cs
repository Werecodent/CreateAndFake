using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.TesterTool.Validators;

namespace Werecodent.CreateAndFake.Tests.TesterTool.Validators;

public sealed class TestValidatorTests() : AutoTester(typeof(TestValidator))
{
    protected override TesterMod OptionConfiguration { get; } =
        opt => opt with { IgnorableExceptions = [typeof(AssertException)] };

    public override Task Tester_NoParameterMutation()
    {
        return Task.CompletedTask;
    }

    [Fact]
    internal static void TestValidator()
    {
        // Name coverage test.
    }
}
