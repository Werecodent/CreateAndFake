using System.Reflection;

namespace Werecodent.CreateAndFake.TesterTool;

#pragma warning disable CA1707, VSTHRD200 // Match test format.

/// <summary>Automatically configures and runs tests for the containing test assembly.</summary>
public abstract class BaseAssemblyTester
{
    /// <summary>Modifications of the <see cref="ITester"/>'s options to apply for the tests.</summary>
    protected virtual TesterMod? OptionConfiguration { get; } = null;

    /// <summary>Reflection tools used to run the tests.</summary>
    /// <value><c>Tools.Source</c></value>
    protected virtual IToolSet AutoTools => Tools.Source;

    /// <summary><see cref="Assembly"/> being tested by the containing test <see cref="Assembly"/>.</summary>
    protected abstract Assembly CodeAssembly { get; }

    /// <inheritdoc cref="TestClassCoverageImplementation"/>
    public abstract void Tester_TestClassCoverage();

    /// <inheritdoc cref="ITester.ProvidesTestClassCoverage"/>
    protected void TestClassCoverageImplementation()
    {
        Tools.Tester.ProvidesTestClassCoverage(
            CodeAssembly,
            GetType().Assembly,
            OptionConfiguration
        );
    }

    /// <inheritdoc cref="ValidateTestMethodNamingImplementation"/>
    public abstract void Tester_ValidateTestMethodNaming();

    /// <inheritdoc cref="ITester.VerifyTestMethodNaming"/>
    protected void ValidateTestMethodNamingImplementation(IEnumerable<Type> testMarkers)
    {
        Tools.Tester.VerifyTestMethodNaming(
            testMarkers,
            CodeAssembly,
            GetType().Assembly,
            OptionConfiguration
        );
    }

    /// <inheritdoc cref="ValidateRandomDataParametersImplementation"/>
    public abstract Task Tester_ValidateRandomDataParameters();

    /// <inheritdoc cref="ITester.ValidateRandomDataParametersAsync"/>
    protected Task ValidateRandomDataParametersImplementation(CancellationToken canceler)
    {
        return Tools.Tester.ValidateRandomDataParametersAsync(
            GetType().Assembly,
            canceler,
            OptionConfiguration
        );
    }

    /// <inheritdoc cref="ValidateAutoTesterInstancesImplementation"/>
    public abstract Task Tester_ValidateAutoTesterInstances();

    /// <inheritdoc cref="ITester.ValidateAutoTesterInstancesAsync"/>
    protected Task ValidateAutoTesterInstancesImplementation(CancellationToken canceler)
    {
        return Tools.Tester.ValidateAutoTesterInstancesAsync(canceler, OptionConfiguration);
    }

    /// <inheritdoc cref="VerifyTestSettingsConfigImplementation"/>
    public abstract Task Tester_VerifyTestSettingsConfig_Default();

    /// <inheritdoc cref="ITester.ValidateTestSettingsConfigAsync"/>
    protected Task VerifyTestSettingsConfigImplementation(CancellationToken canceler)
    {
        return Tools.Tester.ValidateTestSettingsConfigAsync(null, canceler, OptionConfiguration);
    }

    /// <inheritdoc cref="VerifyAllToStringsImplementation"/>
    public abstract void Tester_VerifyAllToStrings();

    /// <inheritdoc cref="ITester.VerifyAllToStrings"/>
    protected void VerifyAllToStringsImplementation()
    {
        Tools.Tester.VerifyAllToStrings(CodeAssembly, OptionConfiguration);
    }

    /// <inheritdoc cref="VerifyToolSetIntegrityImplementation"/>
    public abstract Task Tester_VerifyToolSetIntegrity();

    /// <inheritdoc cref="ITester.VerifyToolSetIntegrityAsync"/>
    protected Task VerifyToolSetIntegrityImplementation(CancellationToken canceler)
    {
        return Tools.Tester.VerifyToolSetIntegrityAsync(canceler, OptionConfiguration);
    }
}

#pragma warning restore
