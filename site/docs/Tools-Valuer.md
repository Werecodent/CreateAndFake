# Valuer

The `Valuer` tool provides methods to compare objects and generate hashcodes by value.

It is recommended to use the `Asserter` tool to test value equality, as it provides detailed failure messages to help debug. Only when the comparison result shouldn't end with a test failure should the `Valuer` be used to compare values.

## Creation & Customization

The `Valuer` requires nothing to be given at creation. Like other tools, custom `CompareHint` instances can be passed in to control behavior for any specific types. Alternatively, interfaces can be attached to the types that enable them to automatically work with the tool.

## IValueEquatable & IValuerEquatable

These interfaces can be attached to objects and enable them to be used with the `Valuer`. `IValueEquatable` specifies the object can compare itself, and `IValuerEquatable` specifies the object can compare itself with the help of the `Valuer` (passed in as a parameter) for child objects.

These interfaces are provided as alternatives to overriding default equality behavior and should follow the same guidelines.
