# Configuration

## Overview

Every tool in the framework has an associated options class with properties (as known as feature flags) that can modify that tool's behavior. These options can be easily configured either via a `testsettings.{environment}.json` configuration file or overridden during method calls.

> ### Available Options
> 
> * <xref:Werecodent.CreateAndFake.Design.Tooling.DesignOptions> (Shared with all tools)
> * <xref:Werecodent.CreateAndFake.AsserterTool.AsserterOptions>
> * <xref:Werecodent.CreateAndFake.DuplicatorTool.DuplicatorOptions>
> * <xref:Werecodent.CreateAndFake.ExtractorTool.ExtractorOptions>
> * <xref:Werecodent.CreateAndFake.FakerTool.FakerOptions>
> * <xref:Werecodent.CreateAndFake.MutatorTool.MutatorOptions>
> * <xref:Werecodent.CreateAndFake.RandomizerTool.RandomizerOptions>
> * <xref:Werecodent.CreateAndFake.RunnerTool.RunnerOptions>
> * <xref:Werecodent.CreateAndFake.TesterTool.TesterOptions>
> * <xref:Werecodent.CreateAndFake.ValuerTool.ValuerOptions>

## Configuration Files

Each option's [[ConfigurableOption]](xref:Werecodent.CreateAndFake.Design.Tooling.ConfigurableOptionAttribute) marked property is set to the value found in order of preference:

1) `testsettings.{environment}.json`
2) `testsettings.json`
3) Its default framework value

Each file is optional. The specific `{environment}` is set via environment variables in order of preference:

1) `DOTNET_ENVIRONMENT`
2) `ASPNETCORE_ENVIRONMENT`
3) Default value of `Production`

> [!TIP]
> Its recommended to always define a basic settings file for easily modifying options later:

> ### `testsettings.json`
> 
> ```json
> {
>     "CreateAndFake": {
>         "Design": {},
>         "Asserter": {},
>         "Duplicator": {},
>         "Extractor": {},
>         "Faker": {},
>         "Mutator": {},
>         "Randomizer": {},
>         "Runner": {},
>         "Tester": {},
>         "Valuer": {}
>     }
> }
> ```

Remember to export the files to be picked up during testing via adding the following to your test project:

> ### {tests}.csproj
>
> ```xml
> <ItemGroup>
>   <Content Include="testsettings.json">
>       <CopyToOutputDirectory>Always</CopyToOutputDirectory>
>   </Content>
> </ItemGroup>
> ```

A fully populated example is available in the source code under:  
`./tests/Configuration.Tests/testsettings.Production.json`

## Call Overrides

All tool methods have an optional settings override parameter that will apply for that call:

> ### Per Method
>
> ```csharp
> text.Assert().Is(original, opt => opt with { 
>   ComparisonModeForStringAsserts = StringComparison.CurrentCulture,
> });
> ```

Similar to customizing method calls, each tool has a `WithOptions` method that will create a new instance with the specified modifications, which becomes the default behavior for all methods on the resulting tool. This can be used to create a locally defined tool for use in a class/method:

> ### Per Tool
>
> ```csharp
> IRandomizer maker = Tools.Randomizer.WithOptions(opt => opt with {
>   StringCharacterSet = ['A'],
> });
> maker.CreateSized<string>(3).Assert().Is("AAA");
> ```

## Substitute Tools

A third configuration method is available, but not recommended for general use. This is done via replacing the default tool implementations with customized versions. <xref:Werecodent.CreateAndFake.Tools.Source?displayProperty=nameWithType> is the origin of all static and fluent <xref:Werecodent.CreateAndFake.Tools> calls, and can be replaced with a <xref:Werecodent.CreateAndFake.ToolSet> configured at runtime.

> [!CAUTION]
> <xref:Werecodent.CreateAndFake.Tools.Source?displayProperty=nameWithType> should only be set once via a [[ModuleInitializer]](xref:System.Runtime.CompilerServices.ModuleInitializerAttribute) or comparable scope.

<xref:Werecodent.CreateAndFake.ToolSet> creation at runtime can be complicated as tools are incorporated into other tools as options themselves. Refer to the <xref:Werecodent.CreateAndFake.ToolSet>'s source code for guidance.
