# Installation

## Overview

There are three levels of packages that can be utilized for different purposes.
1) `Werecodent.CreateAndFake.Design` - Optional support library for non-test projects.
2) `Werecodent.CreateAndFake.Toolbox` - Provides the tools to use in test projects.
3) `Werecodent.CreateAndFake.{UnitTestFramework}` - Attribute support for a unit test framework.

Versions for the packages are synchronized and released alongside each other.

## Instructions

Reference the wanted package using NuGet by following these instructions:

1) For attribute support, identify the `{UnitTestFramework}` you're using from the available options below:
  * `MSTest.v3` (2.2.1 -> 4.0.0)
  * `MSTest.v4` (4.0.0+)
  * `NUnit.v3` (3.10.0+)
  * `xUnit.v2` (2.2.0 -> 3.0.0)
  * `xUnit.v3` (3.0.0+)

2) For attribute support, install using one of the following command (replace `{UnitTestFramework}` with your chosen version):
  ```sh
  dotnet add package Werecodent.CreateAndFake.{UnitTestFramework}
  ```

3) Alternatively, install using the following command for just tool support (already included in attribute support versions):
  ```sh
  dotnet add package Werecodent.CreateAndFake
  ```

4) It's recommended to add the following implicit using lines to your `{tests}.csproj` file for ease of use:
  ```xml
  <ItemGroup>
    <Using Include="Werecodent.CreateAndFake" />
    <Using Include="Werecodent.CreateAndFake.Faking" />
    <Using Include="Werecodent.CreateAndFake.Fluent" />
    <Using Include="Werecodent.CreateAndFake.{UnitTestFramework}" />
  </ItemGroup>
  ```
