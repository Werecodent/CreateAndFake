using System.Reflection;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.TesterTool.Validators;

namespace Werecodent.CreateAndFake.Tests.TesterTool.Validators;

public sealed class SupportValidatorTests() : AutoTester(typeof(SupportValidator))
{
    protected override TesterMod OptionConfiguration { get; } =
        opt =>
            opt with
            {
                DisableNullRefExceptionTests = true,
                DisableParameterMutationTests = true,
                DisableToolSetSupportTests = true,
            };

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
