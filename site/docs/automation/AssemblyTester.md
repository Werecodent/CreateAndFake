# AssemblyTester

## Overview

The <xref:Werecodent.CreateAndFake.xUnit.v3.AssemblyTester> plays a vital role in the framework. Because test data generation happens before tests are ran, issues preventing data generation cause the tests to not run and gives no feedback as to why. So the <xref:Werecodent.CreateAndFake.xUnit.v3.AssemblyTester>, in addition to providing a variety of utility tests, verifies that tests can be generated without issue and details any problems should there be any.

### Setup

Add a `AssemblyNameTests.cs` file (matching the name of your own assembly) with:

> ```csharp
> public sealed class AssemblyNameTests : AssemblyTester
> {
>     protected override Assembly CodeAssembly { get; } =
>         Assembly.GetAssembly(typeof(AClassInTheCodeAssembly));
> }
> ```

If only the base framework is being used, <xref:Werecodent.CreateAndFake.TesterTool.BaseAssemblyTester> can be inherited from instead. Each abstract method required to be overridden has a relevant method to call. See the <xref:Werecodent.CreateAndFake.xUnit.v3.AssemblyTester> source code for an example on how to implement.

To tweak the options used for the tests, override the <xref:Werecodent.CreateAndFake.xUnit.v3.AssemblyTester.OptionConfiguration?displayProperty=nameWithType> like so:

> ```csharp
> protected override Func<TesterOptions, TesterOptions> OptionConfiguration { get; } =
>   opt => opt with { TestClassNameSuffixes = ["Test", "Tests", "TestBase"] };
> ```

View the <xref:Werecodent.CreateAndFake.TesterTool.TesterOptions> for available options along with the documentation for the <xref:Werecodent.CreateAndFake.xUnit.v3.AssemblyTester> methods for which options are relevant to each test. Each test method can also be overridden if necessary.

## Tests


