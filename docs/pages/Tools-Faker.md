# Faker

The `Faker` tool provides fakes to imitate object behavior for testing:

* `Mock` - Strict fake where calls throw exceptions if not set up.
* `Stub` - Loose fake where calls return defaults if not set up.

For attributes:

* `[Fake]` - Loose fake with all methods randomized by the `Randomizer`.
* `[Stub]` - Same as regular `Stub` behavior.

## Example

Fakes use a typical lambda based style with a very methodical structure:

```c#
/// <summary>Verifies setup functionality.</summary>
[Fact]
public void Setup_ObjectEquality()
{
    object data = new object();

    Fake<object> fake = Tools.Faker.Mock<object>();
    fake.Setup(
        d => d.Equals(Arg.Any<object>()),
        Behavior.Returns(false, Times.Once));
    fake.Setup(
        d => d.Equals(data),
        Behavior.Returns(true, Times.Once));

    Tools.Asserter.Is(false, fake.Dummy.Equals(new object()));
    Tools.Asserter.Is(true, fake.Dummy.Equals(data));

    fake.VerifyAll(Times.Exactly(2));
}
```

The functionality is based upon only a few components:

* `Arg` - Useful for matching parameters by various conditions.
* `Behavior` - Used to define what happens when the mocked method is called.
* `Times` - Specifies expected number of calls.

## Creation & Customization

The `Faker` requires a `Valuer` to be given at creation, which controls value equality behavior for parameter matching.

## Verifying

There are a variety of options to verify methods were called as expected:

* `Fake.VerifyAll()` - Automatically checks methods based upon `Times` in each Behavior.
* `Fake.VerifyAll(Times)` - Does the above and that the total matches the provided `Times`.
* `Fake.Verify(Times, delegate)` - Verifies the given delegate was called the given `Times`.
* `Fake.VerifyTotalCalls(Times)` - Only verifies the total number of calls made.

## Voids & Generics

There are two special useful types provided:

* `VoidType` - Used to specify void for return type on behaviors.
* `AnyGeneric` - Used to match any generic in setup methods.

## Faking Internal

The assembly containing the internals to fake needs to grant visibility to the dynamic assembly by adding somewhere (typically AssemblyInfo.cs):

```c#
[assembly: InternalsVisibleTo("CreateAndFake.FakerTypes")]
```

## Faking Protected

The fakes can be setup using the string names for protected methods with an array containing the parameters to match. However, note that using `Arg` conditions requires the .Lambda versions to successfully match calls.

```c#
/// <summary>Verifies hints generate the hashes.</summary>
[Fact]
public void GetHashCode_ValidHint()
{
    object data = new object();
    int result = Tools.Randomizer.Create<int>();

    Fake<CompareHint> hint = Tools.Faker.Mock<CompareHint>();
    hint.Setup("Supports",
        new[] { data, data, Arg.LambdaAny<IValuerChainer>() },
        Behavior.Returns(true, Times.Once));
    hint.Setup("GetHashCode",
        new[] { data, Arg.LambdaAny<IValuerChainer>() },
        Behavior.Returns(result, Times.Once));

    Tools.Asserter.Is(result, new Valuer(false, hint.Dummy).GetHashCode(data));
    hint.Verify(Times.Exactly(2));
}
```

## Faking Ref & Out

The special class `OutRef` is used to handle reference behavior:

```c#
/// <summary>Verifies faking works with out and ref arguments.</summary>
[Theory, RandomData]
public void Fake_HandlesOut(string data, string start)
{
    Fake<OutRefSample> fake = Tools.Faker.Mock<OutRefSample>();
    fake.Setup(
        d => d.WithOut(out Arg.AnyRef<string>().Var),
        Behavior.Set((OutRef<string> d) => { d.Var = data; }, Times.Once));
    fake.Setup(
        d => d.WithRef(ref Arg.WhereRef<string>(v => v == start).Var),
        Behavior.Set((OutRef<string> d) => { d.Var = data; }, Times.Once));

    fake.Dummy.WithOut(out string outValue);
    Tools.Asserter.Is(data, outValue);

    string refValue = start;
    fake.Dummy.WithRef(ref refValue);
    Tools.Asserter.Is(data, refValue);

    fake.Verify(2);
}
```
