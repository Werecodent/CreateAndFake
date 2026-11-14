using NUnit.Framework;
using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.NUnit.v3;

/// <inheritdoc/>
public abstract class AssemblyTester : BaseAssemblyTester
{
    /// <inheritdoc/>
    [Test]
    public override void Tester_TestClassCoverage()
    {
        TestClassCoverageImplementation();
    }

    /// <inheritdoc/>
    [Test]
    public override Task Tester_ValidateAutoTesterInstances()
    {
        return ValidateAutoTesterInstancesImplementation(CancellationTokenProvider.Get());
    }

    /// <inheritdoc/>
    [Test]
    public override Task Tester_ValidateRandomDataParameters()
    {
        return ValidateRandomDataParametersImplementation(CancellationTokenProvider.Get());
    }

    /// <inheritdoc/>
    [Test]
    public override void Tester_ValidateTestMethodNaming()
    {
        ValidateTestMethodNamingImplementation([typeof(TestAttribute)]);
    }

    /// <inheritdoc/>
    [Test]
    public override void Tester_VerifyAllToStrings()
    {
        VerifyAllToStringsImplementation();
    }

    /// <inheritdoc/>
    [Test]
    public override Task Tester_VerifyTestSettingsConfig_Default()
    {
        return VerifyTestSettingsConfigImplementation(CancellationTokenProvider.Get());
    }

    /// <inheritdoc/>
    [Test]
    public override Task Tester_VerifyToolSetIntegrity()
    {
        return VerifyToolSetIntegrityImplementation(CancellationTokenProvider.Get());
    }
}
