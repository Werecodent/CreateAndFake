# Asserter

## Overview

Tool to verify <xref:System.Object> states and <xref:System.Type> behavior. Useful for handling common test scenarios.

API Specification: <xref:Werecodent.CreateAndFake.AsserterTool.Asserter>

Options: <xref:Werecodent.CreateAndFake.AsserterTool.AsserterOptions>

> ### Important Types
> 
> * <xref:Werecodent.CreateAndFake.AsserterTool.AssertException> - <xref:System.Exception> <xref:System.Type> used for all failed assertions.

Behavior should be customized via the dependent tools. See the [Valuer](~/docs/tools/Valuer.md) and [Extractor](~/docs/tools/Extractor.md) documentation.

## Debug Tests

Special `Debug()` assertions are available to use. With the default configuration these statements pass, but when <xref:Werecodent.CreateAndFake.AsserterTool.AsserterOptions.TriggerFailureForDebugAsserts?displayProperty=nameWithType> is enabled the statements fail. Useful for creating tests with data that can be manually reviewed via the resulting <xref:Werecodent.CreateAndFake.AsserterTool.AssertException>'s contents, such as the formatting of <see cref="object.ToString"/> implementations.

## Example

As the <xref:Werecodent.CreateAndFake.ValuerTool.Valuer> is hooked into the <xref:Werecodent.CreateAndFake.AsserterTool.Asserter>, common equality assertion failures specify why they fail. Take:

> ```csharp
> [Theory, RandomData]
> internal static void Tools_DataSampleExample(DataSample original, DataSample variant) 
> {
>   original.Assert().Is(variant, "Expected to fail.");
> }
> ```

Which gives a message detailing the differences like:

> ```
> AssertException : Value equality failed for type 'DataSample'.
> Details: Expected to fail.
> Seed: 409083825
> Content: 
> .StringValue.equals -> Expected:<1xrMPHy>, Actual:<QvQTO6f5h>
> .NumberValue.equals -> Expected:<-1226948183>, Actual:<1440774271>
> .CollectionValue[0].equals -> Expected:<Yc55OaNo6t>, Actual:<5WuyXzEtH4be>
> .CollectionValue[1] -> Expected:<'out of range'>, Actual:<bVAwLGG7s6n6>
> ```
