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

## Example

Simply create a test method and call the `Tester` case:

```csharp
/// <summary>Verifies null reference exceptions are prevented.</summary>
[Fact]
public Task ValueRandom_GuardsNulls()
{
    return Tools.Tester.PreventsNullRefException<ValueRandom>();
}
```

> [!WARNING]
>  <xref:Werecodent.CreateAndFake.TesterTool.Tester> methods are not trivial and can be relatively slow.
