using Microsoft.VisualStudio.TestTools.UnitTesting;
using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.MSTest.v3;

/// <inheritdoc/>
[TestClass]
public abstract class AssemblyTester : BaseAssemblyTester
{
    /// <summary>Used to store information that is provided to unit tests for the MSTest framework.</summary>
    public TestContext? TestContext { get; set; }

    /// <inheritdoc/>
    [TestMethod]
    public override void Tester_TestClassCoverage()
    {
        TestClassCoverageImplementation();
    }

    /// <inheritdoc/>
    [TestMethod]
    public override Task Tester_ValidateAutoTesterInstances()
    {
        return ValidateAutoTesterInstancesImplementation(
            TestContext!.CancellationTokenSource.Token
        );
    }

    /// <inheritdoc/>
    [TestMethod]
    public override Task Tester_ValidateRandomDataParameters()
    {
        return ValidateRandomDataParametersImplementation(
            TestContext!.CancellationTokenSource.Token
        );
    }

    /// <inheritdoc/>
    [TestMethod]
    public override void Tester_ValidateTestMethodNaming()
    {
        ValidateTestMethodNamingImplementation([typeof(TestMethodAttribute)]);
    }

    /// <inheritdoc/>
    [TestMethod]
    public override void Tester_VerifyAllToStrings()
    {
        VerifyAllToStringsImplementation();
    }

    /// <inheritdoc/>
    [TestMethod]
    public override Task Tester_VerifyTestSettingsConfig_Default()
    {
        return VerifyTestSettingsConfigImplementation(TestContext!.CancellationTokenSource.Token);
    }

    /// <inheritdoc/>
    [TestMethod]
    public override Task Tester_VerifyToolSetIntegrity()
    {
        return VerifyToolSetIntegrityImplementation(TestContext!.CancellationTokenSource.Token);
    }
}
