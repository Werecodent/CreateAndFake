using System.Reflection;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.TesterTool.Validators;

namespace Werecodent.CreateAndFake.Tests.TesterTool.Validators;

public sealed class SupportValidatorTests() : AutoTester(typeof(SupportValidator))
{
    public override Task Tester_NoParameterMutation()
    {
        return Task.CompletedTask;
    }

    [Fact]
    internal static void VerifyToolSetSupportAsync_FailsWithoutCoverage()
    {
        Tools
            .Tester.Assert(x =>
                x.ProvidesTestClassCoverage(
                    Assembly.GetAssembly(typeof(ToolSet)),
                    Assembly.GetAssembly(typeof(ToolSet))
                )
            )
            .Throws<AssertException>();
    }
}
