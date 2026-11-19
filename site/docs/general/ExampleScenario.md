# Example Scenario

Imagine having an `ISerializable` with tons of properties. How do you verify it actually serializes properly? You'd create an instance, populate all the properties with canned data, then serialize it. But wait, are you sure the data actually serialized? You'll need to deserialize the result, and compare each property with the original to determine if the data made it.

The result is a rather large test you spent more time setting up than actually testing behavior, and then you have issues. The test has to change every time you modify properties on the data class. What happens when a developer forgets to update the serialization behavior and the test when they add a new property? Potentially, your class now doesn't serialize correctly but your test is still green. At base, you only know your code works with that specific instance of canned data.

This library provides a solution:

```c#
/// <summary>Verifies data can roundtrip.</summary>
[Fact]
public void DataHolder_BinarySerializes()
{
    DataHolder original = Tools.Randomizer.Create<DataHolder>();
    IFormatter formatter = new BinaryFormatter();

    using (Stream stream = new MemoryStream())
    {
        formatter.Serialize(stream, original);
        stream.Seek(0, SeekOrigin.Begin);

        Tools.Asserter.ValuesEqual(original, formatter.Deserialize(stream));
    }
}
```
