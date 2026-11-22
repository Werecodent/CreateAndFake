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

While optional, using the fluent interface is the easiest way to manage fakes, although not available for `Mock` instances. Simply use the <xref:Werecodent.CreateAndFake.Fluent.FakedExtensions>.`SetupReturn` methods on a fake:

> ### SetupReturn
>
> ```csharp
> [Theory, RandomData]
> internal static void Read_TextFromPage([Stub] IBook book, int page, string text)
> {
>   book.Read(Arg.Any<int>()).SetupReturn(text);
>   book.Read(page).Assert().Is(text);
> }
> ```

## Setting Behavior (Fake<T>)

## Specific Cases
