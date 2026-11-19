# Asserter

The `Asserter` tool provides methods to verify common test scenarios:

* `CheckAll` - Verifies multiple cases and aggregates failure.
* `Throws` - Verifies an exception is thrown.
* `IsEmpty` - Verifies a collection has no elements.
* `IsNotEmpty` - Verifies a collection has elements.
* `HasCount` - Verifies a collection has a certain number of elements.
* `Is` - Short for `ValuesEqual`.
* `IsNot` - Short for `ValuesNotEqual`.
* `ValuesEqual` - Verifies two objects are equal by value.
* `ValuesNotEqual` - Verifies two objects are not equal by value.
* `ReferenceEqual` - Verifies two objects are equal by reference.
* `ReferenceNotEqual` - Verifies two objects are not equal by reference.
* `Fail` - Immediately fails the test.

## Example

```c#
/// <summary>Test that will fail.</summary>
[Theory, RandomData]
public void Tools_DataSampleExample(DataSample original)
{
    Tools.Asserter.Is(original, Tools.Mutator.Variant(original));
}
```

Which gives the following message detailing the differences:

```
Result Message:
Test method Werecodent.CreateAndFake.Tests.ToolsTests.Tools_DataSampleExample threw
exception:
CreateAndFake.AsserterTool.AssertException: Value equality failed
for type 'DataSample'.
Content:
.NestedValue.StringValue -> Expected:<CYTsi8X>, Actual:<0RIXwSBI>
.NestedValue.NumberValue -> Expected:<-1982040207>, Actual:<2375491997>
```

## Creation & Customization

The `Asserter` requires a `Valuer` which controls the equality testing behavior.

Unlike the other tools, the `Asserter` can be subclassed to add new methods or override any method to provide different behavior.
