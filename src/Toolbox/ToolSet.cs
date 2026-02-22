using CreateAndFake.AsserterTool;
using CreateAndFake.Design.Randomization;
using CreateAndFake.DuplicatorTool;
using CreateAndFake.ExtractorTool;
using CreateAndFake.FakerTool;
using CreateAndFake.MutatorTool;
using CreateAndFake.RandomizerTool;
using CreateAndFake.RunnerTool;
using CreateAndFake.TesterTool;
using CreateAndFake.ValuerTool;
using Microsoft.Extensions.Configuration;

namespace CreateAndFake;

/// <summary>Holds implementations of all reflection tools.</summary>
/// <param name="gen"><inheritdoc cref="Gen" path="/summary"/></param>
/// <param name="valuer"><inheritdoc cref="Valuer" path="/summary"/></param>
/// <param name="faker"><inheritdoc cref="Faker" path="/summary"/></param>
/// <param name="randomizer"><inheritdoc cref="Randomizer" path="/summary"/></param>
/// <param name="extractor"><inheritdoc cref="Extractor" path="/summary"/></param>
/// <param name="mutator"><inheritdoc cref="Mutator" path="/summary"/></param>
/// <param name="asserter"><inheritdoc cref="Asserter" path="/summary"/></param>
/// <param name="duplicator"><inheritdoc cref="Duplicator" path="/summary"/></param>
/// <param name="runner"><inheritdoc cref="Runner" path="/summary"/></param>
/// <param name="tester"><inheritdoc cref="Tester" path="/summary"/></param>
public sealed class ToolSet(
    IRandom gen,
    IValuer valuer,
    IFaker faker,
    IRandomizer randomizer,
    IExtractor extractor,
    IMutator mutator,
    IAsserter asserter,
    IDuplicator duplicator,
    IRunner runner,
    ITester tester
)
{
    /// <summary>For loading environment specific settings.</summary>
    private static readonly string _EnvironmentName = FindEnvironmentName();

    /// <summary>Finds the configured environment name.</summary>
    /// <returns>The name if found, <c>Production</c> otherwise.</returns>
    internal static string FindEnvironmentName()
    {
        IConfigurationRoot config = new ConfigurationBuilder().AddEnvironmentVariables().Build();

        return config.GetValue<string>("ASPNETCORE_ENVIRONMENT")
            ?? config.GetValue<string>("DOTNET_ENVIRONMENT")
            ?? "Production";
    }

    /// <summary>Default tools to use.</summary>
    public static ToolSet DefaultSet { get; } = CreateViaConfig();

    /// <summary>Creates all the reflection tools using configuration settings.</summary>
    /// <returns>The created reflection tools.</returns>
    public static ToolSet CreateViaConfig()
    {
        IConfigurationSection config = new ConfigurationBuilder()
            .AddJsonFile("testsettings.json", true)
            .AddJsonFile($"testsettings.{_EnvironmentName}.json", true)
            .Build()
            .GetSection("CreateAndFake");

        return Create(config.GetValue("Seed", Environment.TickCount), config);
    }

    /// <summary>Creates all the reflection tools using <paramref name="seed"/>.</summary>
    /// <param name="seed"><inheritdoc cref="SeededRandom(int?)" path="/param[@name='seed']"/></param>
    /// <returns>The created reflection tools.</returns>
    public static ToolSet CreateViaSeed(int seed)
    {
        return Create(seed, null);
    }

    /// <summary>Creates all the reflection tools using the seed and configuration settings.</summary>
    /// <param name="seed"><inheritdoc cref="SeededRandom(int?)" path="/param[@name='seed']"/></param>
    /// <param name="config">Loaded configuration to use.</param>
    /// <returns>The created reflection tools.</returns>
    private static ToolSet Create(int seed, IConfigurationSection? config)
    {
        IRandom gen = new SeededRandom(
            config
                ?.GetSection(nameof(Valuer))
                .GetValue(nameof(ValuerOptions.IterationLimit), 100000)
                ?? 100000,
            !config
                ?.GetSection(nameof(Randomizer))
                .GetValue(nameof(RandomizerOptions.IncludeInfinityAndNaNGeneration), false)
                ?? false,
            seed
        );

        Valuer valuer = new(new ValuerOptions { Gen = gen }.WithConfig(config));
        Faker faker = new(new FakerOptions { Gen = gen, Valuer = valuer }.WithConfig(config));
        Randomizer randomizer = new(
            new RandomizerOptions { Gen = gen, Faker = faker }.WithConfig(config)
        );
        Extractor extractor = new(
            new ExtractorOptions
            {
                Gen = gen,
                Randomizer = randomizer,
                Valuer = valuer,
            }.WithConfig(config)
        );
        Mutator mutator = new(
            new MutatorOptions
            {
                Gen = gen,
                Randomizer = randomizer,
                Valuer = valuer,
                Extractor = extractor,
            }.WithConfig(config)
        );
        Asserter asserter = new(
            new AsserterOptions
            {
                Gen = gen,
                Extractor = extractor,
                Valuer = valuer,
            }.WithConfig(config)
        );
        Duplicator duplicator = new(
            new DuplicatorOptions
            {
                Gen = gen,
                Asserter = asserter,
                Extractor = extractor,
                Valuer = valuer,
            }.WithConfig(config)
        );
        Runner runner = new(
            new RunnerOptions
            {
                Gen = gen,
                Faker = faker,
                Randomizer = randomizer,
                Mutator = mutator,
                Duplicator = duplicator,
            }.WithConfig(config)
        );
        Tester tester = new(
            new TesterOptions
            {
                Gen = gen,
                Randomizer = randomizer,
                Duplicator = duplicator,
                Extractor = extractor,
                Asserter = asserter,
                Runner = runner,
            }.WithConfig(config)
        );

        return new ToolSet(
            gen,
            valuer,
            faker,
            randomizer,
            extractor,
            mutator,
            asserter,
            duplicator,
            runner,
            tester
        );
    }

    /// <inheritdoc cref="IRandom"/>
    public IRandom Gen { get; } = gen;

    /// <inheritdoc cref="IValuer"/>
    public IValuer Valuer { get; } = valuer;

    /// <inheritdoc cref="IFaker"/>
    public IFaker Faker { get; } = faker;

    /// <inheritdoc cref="IRandomizer"/>
    public IRandomizer Randomizer { get; } = randomizer;

    /// <inheritdoc cref="IExtractor"/>
    public IExtractor Extractor { get; } = extractor;

    /// <inheritdoc cref="IMutator"/>
    public IMutator Mutator { get; } = mutator;

    /// <inheritdoc cref="IAsserter"/>
    public IAsserter Asserter { get; } = asserter;

    /// <inheritdoc cref="IDuplicator"/>
    public IDuplicator Duplicator { get; } = duplicator;

    /// <inheritdoc cref="IRunner"/>
    public IRunner Runner { get; } = runner;

    /// <inheritdoc cref="ITester"/>
    public ITester Tester { get; } = tester;
}
