# Getting Started

## Fluent

Text.

## Static

Text.

## Old

Welcome to the documentation source for developers. 

## Quick Start

A default set of all tools are ready to use under the static class Tools.

```c#
/// <summary>Verifies that the tools integrate together.</summary>
[Fact]
public void Tools_IntegrationWorks()
{
    DataSample original = Tools.Randomizer.Create<DataSample>();
    DataSample variant = Tools.Mutator.Variant(original);
    DataSample dupe = Tools.Duplicator.Copy(original);

    Tools.Asserter.Is(original, dupe);
    Tools.Asserter.IsNot(original, variant);

    Fake<DataSample> faked = Tools.Faker.Mock<DataSample>();
    faked.Setup(
        d => d.HasNested(dupe),
        Behavior.Returns(true, Times.Once));

    Tools.Asserter.Is(true, faked.Dummy.HasNested(original),
        "Value equality did not work for args.");

    faked.Verify(Times.Once);
}
```

The tools are designed to cover many common scenarios including immutable classes, classes with private fields, and classes with static factories instead of constructors.

## Customization

In case the default behavior is not enough, each tool can be customized for individual needs by creating your own instances. Additionally, interfaces have been provided for data classes to properly behave with the tools instead. Visit any tool's page to be guided on either route.

Although each tool can be created and customized on a per test basis, the global tools can be modified via a 'ModuleInitializer':

```c#
[ModuleInitializer]
internal static void ModifyTools()
{
    Tools.Source = ToolSet.CreateViaSeed(100);

    Tools.Randomizer.AddHint(new StringCreateHint());
}
```

This way, any customization changes are picked up throughout your code.

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

# IRandom

The `IRandom` classes provide primitive value type randomization without reflection:

```c#
IRandom gen = new FastRandom();
double value1 = gen.Next<double>();
int value2 = gen.Next<int>(0, 5);
```

Provided are three default implementations:

* `FastRandom` - Uses a base that is quick but not cryptographically secure.
* `SecureRandom` - Uses a base that is cryptographically secure but slow.
* `SeededRandom` - Generates deterministic values.

By default only valid values are generated (not NaN or infinities), but that can be changed through the constructor.

## Deterministic

Providing an initial seed and using the `SeededRandom` enables one to get the same random numbers generated between runs, appearing random but giving consistent results:

```c#
/// <summary>Verifies values are deterministic based upon seed.</summary>
[Fact]
public void Seed_Deterministic()
{
    SeededRandom gen = new SeededRandom(5);
    
    Tools.Asserter.Is(-825260579, gen.Next<int>());
    Tools.Asserter.Is(1292641934, gen.Next<int>());
    Tools.Asserter.Is(140109521, gen.Next<int>());

    Tools.Asserter.Is(5, gen.InitialSeed);
    Tools.Asserter.Is(1338796877, gen.Seed);
}
```
