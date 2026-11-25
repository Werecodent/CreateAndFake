# Duplicator

## Overview

Tool to deep clone objects. Useful for tracking mutations by comparing against the created clones.

API Specification: <xref:Werecodent.CreateAndFake.DuplicatorTool.IDuplicator>

Options: <xref:Werecodent.CreateAndFake.DuplicatorTool.DuplicatorOptions>

> ### Important Types
> 
> * <xref:Werecodent.CreateAndFake.DuplicatorTool.Engine.ICopyHint> - Customizes <xref:Werecodent.CreateAndFake.DuplicatorTool.Duplicator> behavior for a specific <xref:System.Type>.
> * <xref:Werecodent.CreateAndFake.DuplicatorTool.Engine.CopyPriority> - Values specifying running order for dynamically loaded hints.
> * <xref:Werecodent.CreateAndFake.Design.Comparisons.IDeepCloneable`1> - Specifies an instance can deep clone itself.
> * <xref:Werecodent.CreateAndFake.DuplicatorTool.IDuplicatable`1> - Specifies an instance can deep clone itself with the help of the <xref:Werecodent.CreateAndFake.DuplicatorTool.Duplicator> (passed in as a parameter) for child objects.

As a hint based tool, <xref:Werecodent.CreateAndFake.DuplicatorTool.Duplicator> behavior provides customization via implementing an <xref:Werecodent.CreateAndFake.DuplicatorTool.Engine.ICopyHint> class. With the default configuration, all found hints in the test assembly are loaded into the tool.

## Example

A common scenario for this behavior is verifying a method doesn't change the values of an instance:

```csharp
/// <summary>Verifies that the method won't mutate the object.</summary>
[Theory, RandomData]
public void SomeMethod_ValuesUnchanged(DataSample original)
{
    DataSample dupe = Tools.Duplicator.Copy(original);

    SomeMethod(original);

    Tools.Asserter.ValuesEqual(original, dupe);
}
```
