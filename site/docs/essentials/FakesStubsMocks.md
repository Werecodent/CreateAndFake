# Fakes & Stubs & Mocks

## Terminology

For the framework, refer to the following definitions for faked objects:

> ### Types
>
> * `Fake` - An instance where calls return random data if not set up.
> * `Mock` - Strict fake where calls throw exceptions if not set up.
> * `Stub` - Loose fake where calls return defaults if not set up.

The <xref:Werecodent.CreateAndFake.FakerTool.IFaker> tool provides mocks and stubs, while the <xref:Werecodent.CreateAndFake.RandomizerTool.IRandomizer> tool can create randomized fakes via <xref:Werecodent.CreateAndFake.FakerTool.Fake`1> instances.

The following test attributes are available as a part of test method generation:

> ### Attributes
>
> * [[Fake]](<xref:Werecodent.CreateAndFake.xUnit.v3.FakeAttribute>)
> * [[Stub]](<xref:Werecodent.CreateAndFake.xUnit.v3.StubAttribute>)

Note that an attribute for mocks is not available due to instability when frameworks try to inspect the instance. Instead, after creation such behavior may be enabled via <xref:Werecodent.CreateAndFake.FakerTool.Fake.ThrowByDefault?displayProperty=nameWithType>:

> ### Behavior Toggle
>
> ```csharp
> ``` db.Tools().ToFake().ThrowByDefault = true;

## Setting Behavior

As with all tools

### Fluent 


## Specific Cases
