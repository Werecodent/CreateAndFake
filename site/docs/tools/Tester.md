# Tester

## Overview

Automates common tests.

API Specification: <xref:Werecodent.CreateAndFake.TesterTool.ITester>

Options: <xref:Werecodent.CreateAndFake.TesterTool.TesterOptions>

> ### Important Types
> 
> * <xref:Werecodent.CreateAndFake.TesterTool.BaseAssemblyTester> - Automation implementation to be inherited by test frameworks.
> * <xref:Werecodent.CreateAndFake.TesterTool.BaseAutoTester`1> - Automation implementation to be inherited by test frameworks.
> * <xref:Werecodent.CreateAndFake.TesterTool.BaseAutoTester> - Automation implementation to be inherited by test frameworks.

> [!NOTE]
> Automated testing has a dedicated documentation pages here: [AssemblyTester](~/docs/automation/AssemblyTester.md) & [AutoTester](~/docs/automation/AutoTester.md)

## Old

The `Tester` tool provides methods to automatically handle common scenarios:

* `PreventsNullRefException` - Verifies nulls are guarded.
* `MutationGuarder` - Verifies parameters aren't mutated by methods.

Be warned that these methods are not trivial and can be relatively slow.

## Example

Simply create a test method and call the `Tester` case:

```c#
/// <summary>Verifies null reference exceptions are prevented.</summary>
[Fact]
public Task ValueRandom_GuardsNulls()
{
    return Tools.Tester.PreventsNullRefException<ValueRandom>();
}
```

## Creation & Customization

The `Tester` requires the other tools to automatically set up and run the tests.

Unlike the other tools, the `Tester` can be subclassed to add new methods or override any method to provide different behavior.
