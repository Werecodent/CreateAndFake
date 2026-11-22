# Randomizer

## Overview

Tool to create objects populated with random values. Useful for generating data to test with.

API Specification: <xref:Werecodent.CreateAndFake.RandomizerTool.IRandomizer>

Options: <xref:Werecodent.CreateAndFake.RandomizerTool.RandomizerOptions>

> ### Important Types
> 
> * <xref:Werecodent.CreateAndFake.RandomizerTool.Engine.ICreateHint> - Customizes <xref:Werecodent.CreateAndFake.RandomizerTool.Randomizer> behavior for a specific <xref:System.Type>.
> * <xref:Werecodent.CreateAndFake.RandomizerTool.Engine.CreatePriority> - Values specifying running order for dynamically loaded hints.

As a hint based tool, <xref:Werecodent.CreateAndFake.RandomizerTool.Randomizer> behavior provides customization via implementing an <xref:Werecodent.CreateAndFake.RandomizerTool.Engine.ICreateHint> class. With the default configuration, all found hints in the test assembly are loaded into the tool.

## Old

The `Randomizer` tool provides methods to create instances populated with random data, as well as injection behavior.

## Example

Typically, many tests will verify behavior based upon specific values on an object. Randomizing an object then specifying the value to test keeps the scope of the test clearly defined and ensures none of the other values matter as expected:

```c#
/// <summary>Verifies the method can't find a child on null.</summary>
[Fact]
public void FindFirstChild_NullChildrenThrows()
{
    DataSample item = Tools.Randomizer.Create<DataSample>();
    item.Children = null;

    Tools.Asserter.Throws<ArgumentNullException>(() => item.FindFirstChild());
}
```

## Injection

Using the inject functionality, the `Randomizer` can create a type using provided values and falling back to randomization for any necessary data not provided. When utilizing the `CreateFor` functionality via attributes, fakes are automatically injected into objects created afterwards. For example:

```C#
[Theory, RandomData]
public void Test([Fake] Child a, Parent b) { }
```

If `Parent` requires an instance of `Child` in the constructor, `a` will be used to create `b`.

## Creation & Customization

The `Randomizer` requires a `Randomizer` to provide stubbing and an `IRandom` instance which controls the core value randomization. Like other tools, custom `CreateHint` instances can be passed in to control behavior for any specific types.

By passing a `SeededRandom` during construction, the `Randomizer` can make tests random but deterministic so that results can be repeated to help track down issues. See the `IRandom` page for more details.
