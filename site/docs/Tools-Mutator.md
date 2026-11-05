# Mutator

The `Mutator` tool provides methods to modify objects and create variants.

## Example

A common scenario for this behavior is verifying a method's behavior changes based upon input:

```c#
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

## Creation & Customization

The `Mutator` requires a `Randomizer` which controls the actual creation and randomization of object, a `Valuer` which verifies the created variant is unequal to the original by value, and a `Limiter` which limits the attempts at creating variants.

At the moment, `Mutator` completely relies upon other tools for its behavior. In the future as the behavior expands, it will work like other tools in that it can be customized by hints.
