using Werecodent.CreateAndFake.TesterTool;
using Xunit;

namespace Werecodent.CreateAndFake.xUnit.v2;

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
        return ValidateAutoTesterInstancesImplementation(CancellationToken.None);
    }

    /// <inheritdoc/>
    [Fact]
    public override Task Tester_ValidateRandomDataParameters()
    {
        return ValidateRandomDataParametersImplementation(CancellationToken.None);
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
        return VerifyTestSettingsConfigImplementation(CancellationToken.None);
    }

    /// <inheritdoc/>
    [Fact]
    public override Task Tester_VerifyToolSetIntegrity()
    {
        return VerifyToolSetIntegrityImplementation(CancellationToken.None);
    }
}
