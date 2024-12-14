using CreateAndFake.Design.Randomization;
using CreateAndFake.Toolbox.AsserterTool;
using CreateAndFake.Toolbox.DuplicatorTool;
using CreateAndFake.Toolbox.FakerTool;
using CreateAndFake.Toolbox.MutatorTool;
using CreateAndFake.Toolbox.RandomizerTool;
using CreateAndFake.Toolbox.TesterTool;
using CreateAndFake.Toolbox.ValuerTool;

namespace CreateAndFake;

/// <summary>Holds implementations of all reflection tools.</summary>
/// <param name="gen"><inheritdoc cref="IRandom" path="/summary"/></param>
/// <param name="valuer"><inheritdoc cref="IValuer" path="/summary"/></param>
/// <param name="faker"><inheritdoc cref="IFaker" path="/summary"/></param>
/// <param name="randomizer"><inheritdoc cref="IRandomizer" path="/summary"/></param>
/// <param name="mutator"><inheritdoc cref="IMutator" path="/summary"/></param>
/// <param name="asserter"><inheritdoc cref="Asserter" path="/summary"/></param>
/// <param name="duplicator"><inheritdoc cref="IDuplicator" path="/summary"/></param>
/// <param name="tester"><inheritdoc cref="Tester" path="/summary"/></param>
public sealed class ToolSet(
    IRandom gen,
    IValuer valuer,
    IFaker faker,
    IRandomizer randomizer,
    IMutator mutator,
    IAsserter asserter,
    IDuplicator duplicator,
    Tester tester)
{
    /// <summary>Default tools to use.</summary>
    public static ToolSet DefaultSet { get; } = CreateViaSeed(Environment.TickCount);

    /// <summary>Creates all the reflection tools using <paramref name="seed"/>.</summary>
    /// <param name="seed"><inheritdoc cref="SeededRandom(int?)" path="/param[@name='seed']"/></param>
    /// <returns>The created reflection tools.</returns>
    public static ToolSet CreateViaSeed(int seed)
    {
        IRandom gen = new SeededRandom(seed);
        IValuer valuer = new Valuer(new ValuerOptions());
        IFaker faker = new Faker(new FakerOptions { Valuer = valuer });
        IRandomizer randomizer = new Randomizer(new RandomizerOptions { Gen = gen, Faker = faker });
        IMutator mutator = new Mutator(new MutatorOptions { Randomizer = randomizer, Valuer = valuer });
        IAsserter asserter = new Asserter(new AsserterOptions { Gen = gen, Valuer = valuer });
        IDuplicator duplicator = new Duplicator(new DuplicatorOptions { Asserter = asserter });
        Tester tester = new(new TesterOptions { Gen = gen, Randomizer = randomizer, Duplicator = duplicator, Asserter = asserter });

        return new ToolSet(gen, valuer, faker, randomizer, mutator, asserter, duplicator, tester);
    }

    /// <inheritdoc cref="IRandom"/>
    public IRandom Gen { get; } = gen;

    /// <inheritdoc cref="IValuer"/>
    public IValuer Valuer { get; } = valuer;

    /// <inheritdoc cref="IFaker"/>
    public IFaker Faker { get; } = faker;

    /// <inheritdoc cref="IRandomizer"/>
    public IRandomizer Randomizer { get; } = randomizer;

    /// <inheritdoc cref="IMutator"/>
    public IMutator Mutator { get; } = mutator;

    /// <inheritdoc cref="IAsserter"/>
    public IAsserter Asserter { get; } = asserter;

    /// <inheritdoc cref="IDuplicator"/>
    public IDuplicator Duplicator { get; } = duplicator;

    /// <inheritdoc cref="Toolbox.TesterTool.Tester"/>
    public Tester Tester { get; } = tester;
}
