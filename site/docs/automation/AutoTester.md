# AutoTester

## Overview

The <xref:Werecodent.CreateAndFake.xUnit.v3.AutoTester`1> automates many common test scenarios, providing a baseline for test coverage immediately and saving a ton of development time without creating any technical debt.

### Setup

Inherit the generic <xref:Werecodent.CreateAndFake.xUnit.v3.AutoTester`1> for visible instance types or the non-generic <xref:Werecodent.CreateAndFake.xUnit.v3.AutoTester> for others:

> ```csharp
> // Preferred for instance types:
> public sealed class DataSampleTests : AutoTester<DataSample>;
>
> // For static or non-public types:
> public sealed class DataSampleTests() : AutoTester(typeof(DataSample));
> ```

It's that simple. If only the base framework is being used, create your own `AutoTester` implementation by inheriting <xref:Werecodent.CreateAndFake.TesterTool.BaseAutoTester`1>. Each abstract method required to be overridden has a relevant method to call. See the <xref:Werecodent.CreateAndFake.xUnit.v3.AutoTester`1> source code for an example on how to implement.

To tweak the options used for the tests, override the <xref:Werecodent.CreateAndFake.TesterTool.BaseAutoTester.OptionConfiguration?displayProperty=nameWithType> like so:

> ```csharp
> protected override Func<TesterOptions, TesterOptions> OptionConfiguration { get; } = 
>   opt => opt with
>   {
>     DisableParameterMutationTests = true,
>     IgnorableExceptions =
>     [
>       typeof(ArgumentException),
>       typeof(InvalidOperationException),
>     ],
>   };
> ```

View the <xref:Werecodent.CreateAndFake.TesterTool.TesterOptions> for available options along with the documentation for the <xref:Werecodent.CreateAndFake.xUnit.v3.AutoTester`1> methods for which options are relevant to each test. Each test method can also be overridden if necessary.

## Provided Tests

Each test is outlined below with helpful information.

### - Tester_GuardsNulls()

Verifies each parameter, that when passed `null`, does not throw a <xref:System.NullReferenceException>. The most common configuration change required to pass this test is adding an <xref:System.Exception> to <xref:Werecodent.CreateAndFake.TesterTool.TesterOptions.IgnorableExceptions?displayProperty=nameWithType> or toggling <xref:Werecodent.CreateAndFake.TesterTool.TesterOptions.IgnoreAllExceptions?displayProperty=nameWithType>. This test is complex and can be slow.

### - Tester_NoParameterMutation()

Verifies each parameter given to the <xref:System.Type> is not mutated by the implemented behavior. In addition to potentially needing to add exceptions to the configuration like `Tester_GuardsNulls`, a common configuration change required to pass this test is adding a specific value to be injected via <xref:Werecodent.CreateAndFake.TesterTool.TesterOptions.InjectionValues?displayProperty=nameWithType>. This test is complex and can be slow. Disable or exclude methods if they intentionally mutate parameters.

> [!CAUTION]
> Beware parameter values that can cause long delays, running times or infinite loops in the implementation. Inject specific values that don't trigger these cases when necessary.

### - Tester_ValueEqualityOverrideCheck()

If the <xref:System.Type> overrides the equality methods, this test verifies they've been overridden correctly.

### - Tester_ToolSupport()

Verifies the <xref:System.Type> can be used freely with the `Create & Fake` tools without any issues. See the relevant tool if any issue arises or disable if tool support isn't necessary for the <xref:System.Type>.

### - Tester_Debug_ToString()

Calls `.ToString().Assert().Debug()` on a randomized instance if possible. See the debug topic in [Asserter](~/docs/tools/Asserter.md#debug-tests) for more details.
