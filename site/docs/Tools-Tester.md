# Tester

The `Tester` tool provides methods to automatically handle common scenarios:

* `PreventsNullRefException` - Verifies nulls are guarded.
* `MutationGuarder` - Verifies paramters aren't mutated by methods.

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
