# Migrating v1 -> v2

This doc details the changes necessary to migrate a code base using 1.x.x versions to version 2.0.0.

No changes are needed if the code base compiles. Changes are mainly required for code customizing the tools instead of using the default set with their default configurations.

## Why

In general, backwards compatibility is a high priority for normal releases. This leads to wanting new behavior be customizable and restricted to changes not breaking the existing API. Tools are highly configurable, but the methodology was found to be lacking especially when only wanting a change on a case by case basis. Using configuration records with optional fluent per method customization resolves these issues.

## Randomizer Changes

```c#
/// <summary>Test that will fail.</summary>
[Theory, RandomData]
public void Tools_DataSampleExample(DataSample original)
{
    Tools.Asserter.Is(original, Tools.Mutator.Variant(original));
}
```
