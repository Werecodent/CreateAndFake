# Duplicator

The `Duplicator` tool provides methods to deep clone.

## Example

A common scenario for this behavior is verifying a method doesn't change the values of an instance:

```c#
/// <summary>Verifies that the method won't mutate the object.</summary>
[Theory, RandomData]
public void SomeMethod_ValuesUnchanged(DataSample original)
{
    DataSample dupe = Tools.Duplicator.Copy(original);

    SomeMethod(original);

    Tools.Asserter.ValuesEqual(original, dupe);
}
```

## Creation & Customization

The `Duplicator` requires an `Asserter` which verifies the created copy is equal to the original by value. Like other tools, custom `CopyHint` instances can be passed in to control behavior for any specific types. Alternatively, interfaces can be attached to the types that enable them to automatically work with the tool.

## IDeepCloneable & IDuplicatable

These interfaces can be attached to objects and enable them to be used with the `Duplicator`. `IDeepCloneable` specifies the object can deep clone itself, and `IDuplicatable` specifies the object can deep clone itself with the help of the `Duplicator` (passed in as a parameter) for child objects.
