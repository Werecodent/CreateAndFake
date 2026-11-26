# AssemblyTester

## Overview

The <xref:Werecodent.CreateAndFake.xUnit.v3.AssemblyTester> plays a vital role in the framework. Because test data generation happens before tests are ran, issues preventing data generation cause the tests to not run and gives no feedback as to why. So the <xref:Werecodent.CreateAndFake.xUnit.v3.AssemblyTester>, in addition to providing a variety of utility tests, verifies that tests can be generated without issue and details any problems should there be any.

### Setup

Add an `AssemblyNameTests.cs` file (matching the name of your own assembly) with:

> ```csharp
> public sealed class AssemblyNameTests : AssemblyTester
> {
>     protected override Assembly CodeAssembly { get; } =
>         Assembly.GetAssembly(typeof(AClassInTheCodeAssembly));
> }
> ```

If only the base framework is being used, <xref:Werecodent.CreateAndFake.TesterTool.BaseAssemblyTester> can be inherited from instead. Each abstract method required to be overridden has a relevant method to call. See the <xref:Werecodent.CreateAndFake.xUnit.v3.AssemblyTester> source code for an example on how to implement.

To tweak the options used for the tests, override the <xref:Werecodent.CreateAndFake.TesterTool.BaseAssemblyTester.OptionConfiguration?displayProperty=nameWithType> like so:

> ```csharp
> protected override Func<TesterOptions, TesterOptions> OptionConfiguration { get; } =
>   opt => opt with { TestClassNameSuffixes = ["Test", "Tests", "TestBase"] };
> ```

View the <xref:Werecodent.CreateAndFake.TesterTool.TesterOptions> for available options along with the documentation for the <xref:Werecodent.CreateAndFake.xUnit.v3.AssemblyTester> methods for which options are relevant to each test. Each test method can also be overridden if necessary.

## Provided Tests

Each test is outlined below with helpful information.

### - Tester_VerifyTestSettingsConfig_Default()

Ensures a `testsettings.json` file is created and has no invalid values. See [Configuration](~/docs/introduction/Configuration.md) for more details. Highly recommended to not disable.

> [!TIP]
> Add a test calling `VerifyTestSettingsConfigImplementation` for each `testsettings.{environment}.json` you're using!

### - Tester_TestClassCoverage()

Enforces every <xref:System.Type> in the <xref:Werecodent.CreateAndFake.TesterTool.BaseAssemblyTester?displayProperty=nameWithType> has a matching test class in the executing test assembly. Finds matches by name. Recommended to not disable.

### - Tester_ValidateTestMethodNaming()

Enforces every test method starts with an appropriate `{target}_` prefix. Disable if this behavior is not wanted (toggling <xref:Werecodent.CreateAndFake.TesterTool.TesterOptions.RequirePrefixInTestName?displayProperty=nameWithType> will only enforce if `_` is present). Valid targets are:

> * Name of the code class
> * Name of the code assembly
> * Name of the test assembly
> * "Tester"
> * "New"
> * <xref:Werecodent.CreateAndFake.TesterTool.TesterOptions.AllowedTestNamePrefixes?displayProperty=nameWithType>

### - Tester_VerifyAllToStrings()

Randomizes all found code classes and converts each one to a <xref:System.String> in a `Debug()` assertion. See the debug topic in [Asserter](~/docs/tools/Asserter.md#debug-tests) for more details. Disable if this behavior is not wanted.

### - Tester_ValidateAutoTesterInstances()

Ensures all <xref:Werecodent.CreateAndFake.xUnit.v3.AutoTester> instances can be properly created and run tests. Do not disable.

### - Tester_ValidateRandomDataParameters()

Ensures all methods marked with the <xref:Werecodent.CreateAndFake.xUnit.v3.RandomDataAttribute> can be properly generated. Do not disable.

### - Tester_VerifyToolSetIntegrity()

Ensures the framework is completely functioning in the current environment. Recommended to not disable, please create an [issue](https://github.com/Werecodent/CreateAndFake/issues) for any encountered failures. Temporary exclusions can be set via <xref:Werecodent.CreateAndFake.TesterTool.TesterOptions.IntegrityIgnorableTypes?displayProperty=nameWithType> for known issues.
