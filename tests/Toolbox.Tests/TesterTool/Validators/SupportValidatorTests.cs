using System.Reflection;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.TesterTool.Validators;

namespace Werecodent.CreateAndFake.Tests.TesterTool.Validators;

public static class SupportValidatorTests
{
    [Fact]
    internal static Task SupportValidator_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<SupportValidator>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void VerifyToolSetSupportAsync_FailsWithoutCoverage()
    {
        Tools
            .Tester.Assert(x =>
                x.ProvidesTestClassCoverage(
                    Assembly.GetAssembly(typeof(ToolSet)),
                    Assembly.GetExecutingAssembly()
                )
            )
            .Throws<AssertException>();
    }
}
