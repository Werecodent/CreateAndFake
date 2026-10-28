using System.Reflection;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.FakerTool.Proxy;
using Werecodent.CreateAndFake.Samples.ErrorCases;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests;

public static class ToolsTests
{
    [Fact]
    internal static void CreateAndFake_TestClassCoverage()
    {
        Tools.Tester.ProvidesTestClassCoverage(
            Assembly.GetAssembly(typeof(ToolSet)),
            Assembly.GetExecutingAssembly(),
            opt => opt with { ClassCoverageExclusions = [nameof(Emitter)] }
        );
    }

    [Fact]
    internal static void CreateAndFake_ValidateTestMethodNaming()
    {
        Tools.Tester.VerifyTestMethodNaming(
            [typeof(FactAttribute), typeof(TheoryAttribute)],
            Assembly.GetAssembly(typeof(ToolSet)),
            Assembly.GetExecutingAssembly()
        );
    }

    [Fact]
    internal static Task CreateAndFake_ValidateRandomDataParameters()
    {
        return Tools.Tester.ValidateRandomDataParametersAsync(
            Assembly.GetExecutingAssembly(),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task CreateAndFake_VerifyTestSettingsConfig_Default()
    {
        return Tools.Tester.ValidateTestSettingsConfigAsync(
            null,
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void CreateAndFake_VerifyAllToStrings()
    {
        Tools.Tester.VerifyAllToStrings(Assembly.GetAssembly(typeof(ToolSet)));
    }

    [Fact]
    internal static Task Tools_VerifyIntegrity()
    {
        return Tools.Tester.VerifyToolSetIntegrityAsync(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static void Tools_IntegrationWorks(
        DataHolderSample original,
        [Fake] DataHolderSample faked
    )
    {
        DataHolderSample dupe = original.Tools().Copy();

        original.Assert().Is(dupe).And().IsNot(original.Tools().Variant());

        faked.HasNested(dupe).SetupReturn(true, Times.Once);
        faked.HasNested(original).Assert().Is(true, "Value equality did not work for args.");
        faked.Assert().Called();
    }

    [Theory, RandomData]
    internal static void Tools_HandlesInfinites(InfiniteSample sample)
    {
        Tools.Mutator.Assert(x => x.Variant(sample)).Throws<ToolException>();

        InfiniteSample dupe = Tools.Duplicator.Copy(sample);

        dupe.Assert().Is(sample);
        Tools.Valuer.GetHashCode(dupe).Assert().Is(Tools.Valuer.GetHashCode(sample));
    }

    [Fact]
    internal static async Task Tools_TestIndividual()
    {
        for (int i = 0; i < 100; i++)
        {
            await Tools.Tester.VerifyToolSetSupportAsync(
                [typeof(Dictionary<string, object>)],
                TestContext.Current.CancellationToken
            );
        }
    }
}
