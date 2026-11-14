using Werecodent.CreateAndFake.TesterTool;
using Xunit;

namespace Werecodent.CreateAndFake.xUnit.v3;

/// <inheritdoc/>
public abstract class AssemblyTester : BaseAssemblyTester
{
    /// <inheritdoc/>
    [Fact]
    public override void Tester_TestClassCoverage()
    {
        TestClassCoverageImplementation();
    }

    /// <inheritdoc/>
    [Fact]
    public override Task Tester_ValidateAutoTesterInstances()
    {
        return ValidateAutoTesterInstancesImplementation(TestContext.Current.CancellationToken);
    }

    /// <inheritdoc/>
    [Fact]
    public override Task Tester_ValidateRandomDataParameters()
    {
        return ValidateRandomDataParametersImplementation(TestContext.Current.CancellationToken);
    }

    /// <inheritdoc/>
    [Fact]
    public override void Tester_ValidateTestMethodNaming()
    {
        ValidateTestMethodNamingImplementation([typeof(FactAttribute), typeof(TheoryAttribute)]);
    }

    /// <inheritdoc/>
    [Fact]
    public override void Tester_VerifyAllToStrings()
    {
        VerifyAllToStringsImplementation();
    }

    /// <inheritdoc/>
    [Fact]
    public override Task Tester_VerifyTestSettingsConfig_Default()
    {
        return VerifyTestSettingsConfigImplementation(TestContext.Current.CancellationToken);
    }

    /// <inheritdoc/>
    [Fact]
    public override Task Tester_VerifyToolSetIntegrity()
    {
        return VerifyToolSetIntegrityImplementation(TestContext.Current.CancellationToken);
    }
}
