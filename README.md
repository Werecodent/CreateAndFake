# Create & Fake

[![NuGet](https://img.shields.io/nuget/v/CreateAndFake?logo=nuget)](https://www.nuget.org/packages/CreateAndFake) [![Build](https://github.com/Werecodent/CreateAndFake/actions/workflows/integration.yml/badge.svg?branch=main)](https://github.com/Werecodent/CreateAndFake/actions/workflows/integration.yml) [![CodeCov Coverage](https://codecov.io/gh/Werecodent/CreateAndFake/branch/main/graph/badge.svg)](https://codecov.io/gh/Werecodent/CreateAndFake/branch/main) [![GitHub Sponsors](https://img.shields.io/github/sponsors/Werecodent?logo=githubsponsors)](https://github.com/sponsors/Werecodent)

Create & Fake is a test support library, developed by humans, that makes robust testing easy by handling all manners of data generation, mocking, comparisons and validation. Designed to handle the bulk of test setup quickly and easily so that developers can focus on the behavior to test, making tests easier to develop and maintain like so:

> ### Test Attribute Example
>
> ```c#
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

By default, all behavior works not by reference but by value even for complex objects. Using the fluent extension methods and attributes are optional as all functionality are present in customizable tools:

> ### Tool Use Example
>
> ```C#
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
> ```C#
> public sealed class MyClassTests : AutoTester<MyClass>;
> ```

A key benefit of the library is in how the tools are logically integrated with each other. For example, the `Randomizer` will use stubs for interfaces that have no known implementations in the code. Or the mocks created by the `Faker` utilize value equality in matching arguments.

> ### Tool List
>
> * `Asserter` - Verify object states and behavior.
> * `Duplicator` - Deep clone objects.
> * `Extractor` - Flatten the contents of objects.
> * `Faker` - Simulate behavior via stubs/mocks.
> * `Mutator` - Modify the state of objects or create variants.
> * `Randomizer` - Create objects populated with random values.
> * `Runner` - Generate method data and run them.
> * `Tester` - Test common scenarios.
> * `Valuer` - Compare objects by value via reflection if needed.

## Documentation

Visit the [documentation site](https://werecodent.github.io/CreateAndFake/) for more information and how to get started.

A local site can be built from the source code using the terminal command "./build/run.sh serveSite".

## Sponsors

<a href="https://github.com/Werebunny"><img src="https://github.com/Werebunny.png" width="40px" title="Werebunny" alt="Werebunny" /></a>

## Contributing

If you're looking to contribute, thanks for your interest. Feel free to submit reports for any issues you can find, or request potential features you'd like to see [here](../../issues). If you wish to contribute code to the project, refer to the [contributing](https://github.com/Werecodent/.github/blob/main/CONTRIBUTING.md) requirements and the [support](https://github.com/Werecodent/.github/blob/main/SUPPORT.md) guide for environment setup. Please follow the [code of conduct](https://github.com/Werecodent/.github/blob/main/CODE_OF_CONDUCT.md) when contributing.

## License

This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

View the [license](LICENSE.txt) file for more details.

## Acknowledgments

* [xUnit](https://github.com/xunit/xunit) + [NUnit](https://github.com/nunit/nunit) + [MSTest](https://github.com/microsoft/testfx): For running tests.
* [Coverlet](https://github.com/tonerdo/coverlet) + [ReportGenerator](https://danielpalme.github.io/ReportGenerator) + [CodeCov](https://codecov.io): For test coverage.
* [Bullseye](https://github.com/adamralph/bullseye) + [SimpleExec](https://github.com/adamralph/simple-exec) + [MinVer](https://github.com/adamralph/minver): For project building.
* [AsyncFixer](https://github.com/semihokur/AsyncFixer) + [MissingAwaitWarning](https://github.com/ykoksen/unused-task-warning) + [Meziantou.Analyzer](https://github.com/meziantou/Meziantou.Analyzer) + [Sonar](https://github.com/SonarSource/sonar-dotnet): For code analyzers.
* [DocFX](https://github.com/dotnet/docfx): For building the documentation site.
* [CSharpier](https://github.com/belav/csharpier): For formatting code.
* [GitHub](https://github.com): For hosting code.
* [Microsoft](https://visualstudio.microsoft.com/vs/features/net-development): For C# and editors.
