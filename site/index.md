# About

## Overview

Create & Fake is a test support library that makes robust testing easy by handling all manners of data generation, mocking, comparisons and validation. Designed to handle the bulk of test setup quickly and easily so that developers can focus on the behavior to test, making tests easier to develop and maintain like so:

> ### Test Attribute Example
> 
> ```csharp
> // Provided xUnit attributes used to automatically create the parameters.
> [Theory, RandomData]
> internal static void GetDirectors_UtilizesYearToPruneDirectors(
>     [Fake] IStorage db, [Injected] Endpoint api, [Size(2)] Details[] movies)
> {
>     // Setup fake behavior using randomized movie data (times optional).
>     db.Find(movies[0].Name).SetupReturn(movies, Times.Once);
> 
>     // Call code and test for expected result (api is auto-injected with db fake).
>     api.GetDirectors(movies[0].Name, movies[0].Year).Assert().Is(movies[0].Directors);
> 
>     // Optionally check all behavior called as expected.
>     db.Assert().Called();
> }
> ```

By default, all behavior works not by reference but by value even for complex objects. Using the fluent extension methods and attributes are optional as all functionality are present in customizable <xref:Werecodent.CreateAndFake.Tools>:

> ### Tool Use Example
> 
> ```csharp
> [Fact]
> internal static void MailingAddress_VerifyChangingZipCodeUpdatesAddress()
> {
>     // Randomizer uses readable values for some properties like 'FirstName'.
>     User details = Tools.Randomizer.Create<User>();
> 
>     // Duplicator creates deep clones. 
>     User copy = Tools.Duplicator.Copy(details);
> 
>     // Mutator can randomly modify objects too.
>     details.ZipCode = Tools.Mutator.Variant(details.ZipCode);
> 
>     // Asserter performs checks based upon value.
>     Tools.Asserter.IsNot(details.MailingAddress, copy.MailingAddress);
> }
> ```

Even more, classes are able to be automatically tested in a variety of ways simply by inheriting the automated test base:

> ### AutoTester Example
> 
> ```csharp
> public sealed class MyClassTests : AutoTester<MyClass>;
> ```

A key benefit of the library is in how the tools are logically integrated with each other. For example, the <xref:Werecodent.CreateAndFake.RandomizerTool.Randomizer> will use stubs for interfaces that have no known implementations in the code. Or the mocks created by the <xref:Werecodent.CreateAndFake.FakerTool.Faker> utilize value equality in matching arguments.

> ### Tool List
> 
> * <xref:Werecodent.CreateAndFake.AsserterTool.Asserter> - Verify object states and behavior.
> * <xref:Werecodent.CreateAndFake.DuplicatorTool.Duplicator> - Deep clone objects.
> * <xref:Werecodent.CreateAndFake.ExtractorTool.Extractor> - Flatten the contents of objects.
> * <xref:Werecodent.CreateAndFake.FakerTool.Faker> - Simulate behavior via stubs/mocks.
> * <xref:Werecodent.CreateAndFake.MutatorTool.Mutator> - Modify the state of objects or create variants.
> * <xref:Werecodent.CreateAndFake.RandomizerTool.Randomizer> - Create objects populated with random values.
> * <xref:Werecodent.CreateAndFake.RunnerTool.Runner> - Generate method data and run them.
> * <xref:Werecodent.CreateAndFake.TesterTool.Tester> - Test common scenarios.
> * <xref:Werecodent.CreateAndFake.ValuerTool.Valuer> - Compare objects by value via reflection if needed.
