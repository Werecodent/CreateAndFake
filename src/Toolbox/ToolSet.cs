using CreateAndFake.AsserterTool;
using CreateAndFake.AsyncAsserterTool;
using CreateAndFake.Design.Randomization;
using CreateAndFake.DuplicatorTool;
using CreateAndFake.ExtractorTool;
using CreateAndFake.FakerTool;
using CreateAndFake.MutatorTool;
using CreateAndFake.RandomizerTool;
using CreateAndFake.RunnerTool;
using CreateAndFake.TesterTool;
using CreateAndFake.ValuerTool;

namespace CreateAndFake;

/// <summary>Holds implementations of all reflection tools.</summary>
/// <param name="gen"><inheritdoc cref="Gen" path="/summary"/></param>
/// <param name="valuer"><inheritdoc cref="Valuer" path="/summary"/></param>
/// <param name="faker"><inheritdoc cref="Faker" path="/summary"/></param>
/// <param name="randomizer"><inheritdoc cref="Randomizer" path="/summary"/></param>
/// <param name="extractor"><inheritdoc cref="Extractor" path="/summary"/></param>
/// <param name="mutator"><inheritdoc cref="Mutator" path="/summary"/></param>
/// <param name="asserter"><inheritdoc cref="Asserter" path="/summary"/></param>
/// <param name="asyncAsserter"><inheritdoc cref="AsyncAsserter" path="/summary"/></param>
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
    IAsyncAsserter asyncAsserter,
    IDuplicator duplicator,
    IRunner runner,
    ITester tester
)
{
    /// <summary>Default tools to use.</summary>
    public static ToolSet DefaultSet { get; } = CreateViaSeed(Environment.TickCount);

    /// <summary>Creates all the reflection tools using <paramref name="seed"/>.</summary>
    /// <param name="seed"><inheritdoc cref="SeededRandom(int?)" path="/param[@name='seed']"/></param>
    /// <returns>The created reflection tools.</returns>
    public static ToolSet CreateViaSeed(int seed)
    {
        IRandom gen = new SeededRandom(seed);
        Valuer valuer = new(new ValuerOptions());
        Faker faker = new(new FakerOptions { Gen = gen, Valuer = valuer });
        Randomizer randomizer = new(new RandomizerOptions { Gen = gen, Faker = faker });
        Extractor extractor = new(
            new ExtractorOptions { Randomizer = randomizer, Valuer = valuer }
        );
        Mutator mutator = new(
            new MutatorOptions
            {
                Gen = gen,
                Randomizer = randomizer,
                Valuer = valuer,
                Extractor = extractor,
            }
        );
        Asserter asserter = new(
            new AsserterOptions
            {
                Gen = gen,
                Extractor = extractor,
                Valuer = valuer,
            }
        );
        AsyncAsserter asyncAsserter = new(
            new AsyncAsserterOptions
            {
                Gen = gen,
                Extractor = extractor,
                Valuer = valuer,
                Asserter = asserter,
            }
        );
        Duplicator duplicator = new(
            new DuplicatorOptions { Asserter = asserter, Extractor = extractor }
        );
        Runner runner = new(
            new RunnerOptions
            {
                Gen = gen,
                Faker = faker,
                Randomizer = randomizer,
                Mutator = mutator,
                Duplicator = duplicator,
            }
        );
        Tester tester = new(
            new TesterOptions
            {
                Gen = gen,
                Randomizer = randomizer,
                Duplicator = duplicator,
                Asserter = asserter,
                AsyncAsserter = asyncAsserter,
                Runner = runner,
            }
        );

        return new ToolSet(
            gen,
            valuer,
            faker,
            randomizer,
            extractor,
            mutator,
            asserter,
            asyncAsserter,
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

    /// <inheritdoc cref="IAsyncAsserter"/>
    public IAsyncAsserter AsyncAsserter { get; } = asyncAsserter;

    /// <inheritdoc cref="IDuplicator"/>
    public IDuplicator Duplicator { get; } = duplicator;

    /// <inheritdoc cref="IRunner"/>
    public IRunner Runner { get; } = runner;

    /// <inheritdoc cref="ITester"/>
    public ITester Tester { get; } = tester;
}
