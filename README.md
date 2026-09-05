# Create & Fake

[![NuGet](https://img.shields.io/nuget/v/CreateAndFake)](https://www.nuget.org/packages/CreateAndFake) [![Build](https://github.com/Werecodent/CreateAndFake/actions/workflows/integration.yml/badge.svg?branch=main)](https://github.com/Werecodent/CreateAndFake/actions/workflows/integration.yml) [![CodeCov Coverage](https://codecov.io/gh/Werecodent/CreateAndFake/branch/main/graph/badge.svg)](https://codecov.io/gh/Werecodent/CreateAndFake/branch/main)

A C# class library that handles mocking, test data generation, and validation. Designed to handle the bulk of test setup quickly and easily so that developers can focus on the behavior to test, making tests easier to develop and maintain:

```c#
// xUnit attributes used to automatically create the parameters.
[Theory, RandomData]
internal static void TestGetMovieDirectors(
    [Fake] IStorage db, Endpoint api, [Size(2)] Details[] movies)
{
    // Setup fake behavior using randomized movie data (times optional).
    db.Find(movies[0].Name).SetupReturn(movies, Times.Once);

    // Call code and test for expected result (api is auto-injected with db fake).
    api.GetDirectors(movies[0].Name, movies[0].Year).Assert().Is(movies[0].Directors);

    // Optionally check all behavior called as expected.
    db.Verify();
}
```

By default, all behavior works not by reference but by value even for complex objects. Using the extension methods and attributes are optional as all functionality are present in customizable tools:

```C#
[Fact]
internal static void TestChangingZipCodeUpdatesAddress()
{
    // Randomizer uses readable values for some properties like 'FirstName'.
    User details = Tools.Randomizer.Create<User>();

    // Duplicator creates deep clones. 
    User copy = Tools.Duplicator.Copy(details);

    // Mutator can randomly modify objects too.
    details.ZipCode = Tools.Mutator.Variant(details.ZipCode);

    // Asserter performs checks based upon value.
    Tools.Asserter.IsNot(copy.MailingAddress, details.MailingAddress);
}
```

A key benefit of the library is in how the tools are logically integrated with each other. For example, the `Randomizer` will use stubs for interfaces that have no known implementations in the code. Or the mocks created by the `Faker` utilize value equality in matching arguments.

* `Asserter` - Handles common test scenarios.
* `Faker` - Creates mocks and stubs.
* `Randomizer` - Creates random instances of any type.
* `Duplicator` - Creates deep clones of objects.
* `Mutator` - Mutates objects or creates variants.
* `Valuer` - Compares objects by value.
* `Tester` - Automates common tests.

Visit the [documentation site](https://werecodent.github.io/CreateAndFake/) for more information and how to get started.

## Installation

* Install using:

```
dotnet add package CreateAndFake
```

* Use in a class by adding:

```
using Werecodent.CreateAndFake;
using Werecodent.CreateAndFake.Fluent;
```

## Documentation

The documentation site is located here: https://werecodent.github.io/CreateAndFake/

The raw files can be viewed from the doc folder or built into a local site using Jekyll.

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
* [CSharpier](https://github.com/belav/csharpier): For formatting code.
* [GitHub](https://github.com): For hosting code.
* [Microsoft](https://visualstudio.microsoft.com/vs/features/net-development): For C# and editors.
