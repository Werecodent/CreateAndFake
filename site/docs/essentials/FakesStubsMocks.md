# Fakes & Stubs & Mocks

## Terminology

For the framework, refer to the following definitions for faked objects:

> ### Types
>
> * `Fake` - Implementation whose behavior can be set at runtime.
> * `Stub` - Loose fake where calls return defaults if not set up.
> * `Mock` - Strict fake where calls throw exceptions if not set up.

The <xref:Werecodent.CreateAndFake.FakerTool.Faker> tool provides mocks and stubs, while the <xref:Werecodent.CreateAndFake.RandomizerTool.Randomizer> tool can create randomized fakes via <xref:Werecodent.CreateAndFake.FakerTool.Fake`1> wrappers. Access the faked instance itself via  <xref:Werecodent.CreateAndFake.FakerTool.Fake`1.Dummy?displayProperty=nameWithType>.

The following test attributes are available as a part of test method generation that can be attached directly to the wanted type:

> ### Attributes
>
> * [[Fake]](<xref:Werecodent.CreateAndFake.xUnit.v3.FakeAttribute>) - Fake with all behavior randomized.
> * [[Stub]](<xref:Werecodent.CreateAndFake.xUnit.v3.StubAttribute>) - Same as regular `Stub` behavior.

Note that a `Mock` attribute is not available due to instability when frameworks internally try to inspect the instance. Instead, after creation such behavior may be enabled via <xref:Werecodent.CreateAndFake.FakerTool.Fake.ThrowByDefault?displayProperty=nameWithType>:

> ### Behavior Toggle
>
> ```csharp
> [Theory, RandomData]
> internal static void GetItemAsync_ThrowsAsMock([Stub] IStorage db, int id)
> {
>   db.Tools().ToFake().ThrowByDefault = true;
>   db.GetItemAsync(id).Assert().Throws<Exception>();
> }
> ```

Once you have your fake, there are a few special terms designed to make setting up behavior simple:

> ### Special Behavior
>
> * <xref:Werecodent.CreateAndFake.Faking.Arg> - Useful for matching parameters by various conditions.
> * <xref:Werecodent.CreateAndFake.Faking.Behavior> - Used to define what happens when the mocked method is called.
> * <xref:Werecodent.CreateAndFake.Faking.Times> - Specifies expected number of calls.
> * <xref:Werecodent.CreateAndFake.Faking.AnyGeneric> - Used to match any generic in setup methods.
> * <xref:Werecodent.CreateAndFake.VoidType> - Used to specify void for return type on behaviors.
> * <xref:Werecodent.CreateAndFake.Faking.OutRef`1> - Used to match out or ref parameters.

## Setting Behavior (Fluent)

While optional, using the fluent interface is the easiest way to manage fakes, although not available for `Mock` instances or members that aren't visible. Simply use the <xref:Werecodent.CreateAndFake.Fluent.FakedExtensions>.`SetupReturn` methods on a fake:

> ### SetupReturn
>
> ```csharp
> [Theory, RandomData]
> internal static void Read_TextFromPage(
>     [Fake] IBook book, int page, int line, string text)
> {
>   // For methods with returns:  
>   book.Read(page, Arg.Any<int>()).SetupReturn(text);
>   book.Read(page, line).Assert().Is(text);
>
>   // For void methods:
>   book.SetupReturn(x => x.Close(), Behavior.None(Times.Once));
>   book.Close();
>
>   // Verifies all behavior is called as expected.
>   book.Assert().Called(Times.AtLeast(2));
> }
> ```

> [!TIP]
> Values are compared by value when determining what behavior is called for the fake.

## Setting Behavior (Fake<T>)

The <xref:Werecodent.CreateAndFake.FakerTool.Fake`1> is the wrapper for fakes to set behavior. Here's the exact same example as above using the wrapper instead:

> ### Wrapper Setup
>
> ```csharp
> [Theory, RandomData]
> internal static void Read_TextFromPage(
>     Fake<IBook> book, int page, int line, string text)
> {
>   // For methods with returns:  
>   book.Setup(x => x.Read(page, Arg.Any<int>()), Behavior.Returns(text));
>   book.Dummy.Read(page, line).Assert().Is(text);
>
>   // For void methods:
>   book.Setup(x => x.Close(), Behavior.None(Times.Once));
>   book.Dummy.Close();
>
>   // The wrapper may also verify all behavior is called as expected.
>   book.Verify(Times.AtLeast(2));
> }
> ```

## Faking Internal

The assembly containing internals must grant visibility to the dynamic assembly by adding the following somewhere (typically `AssemblyInfo.cs`):

> ```csharp
> [assembly: InternalsVisibleTo("Werecodent.CreateAndFake.FakerTypes")]
> ```

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
