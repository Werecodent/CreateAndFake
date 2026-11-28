# Mutator

## Overview

Tool to modify the state of objects or create variants. Useful for testing behavior under various states.

API Specification: <xref:Werecodent.CreateAndFake.MutatorTool.IMutator>

Options: <xref:Werecodent.CreateAndFake.MutatorTool.MutatorOptions>

> ### Important Types
> 
> * <xref:Werecodent.CreateAndFake.MutatorTool.Engine.IMutateHint> - Customizes <xref:Werecodent.CreateAndFake.MutatorTool.Mutator> behavior for a specific <xref:System.Type>.
> * <xref:Werecodent.CreateAndFake.MutatorTool.Engine.MutatePriority> - Values specifying running order for dynamically loaded hints.

As a hint based tool, <xref:Werecodent.CreateAndFake.MutatorTool.Mutator> behavior provides customization via implementing an <xref:Werecodent.CreateAndFake.MutatorTool.Engine.IMutateHint> class. With the default configuration, all found hints in the test assembly are loaded into the tool.

## Example

A common scenario for this behavior is verifying a method's behavior changes based upon input:

```csharp
/// <summary>Verifies different hashes are generated for different values.</summary>
[Theory, RandomData]
public void GetHashCode_HashVaries(int original)
{
    int variant = Tools.Mutator.Variant(original);

    Tools.Asserter.ValuesNotEqual(
        ValueComparer.Use.GetHashCode(original),
        ValueComparer.Use.GetHashCode(variant));
}
```
