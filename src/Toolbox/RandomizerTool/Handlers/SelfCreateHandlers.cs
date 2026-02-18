using CreateAndFake.AsserterTool;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Reiteration;
using CreateAndFake.DuplicatorTool;
using CreateAndFake.ExtractorTool;
using CreateAndFake.FakerTool.Proxy;
using CreateAndFake.MutatorTool;
using CreateAndFake.RandomizerTool.Engine;
using CreateAndFake.RandomizerTool.Hints;
using CreateAndFake.RunnerTool;
using CreateAndFake.TesterTool;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.RandomizerTool.Handlers;

internal static class SelfCreateHandlers
{
    private static readonly ObjectCreateHint _SubHint = new();

    /// <summary>Supported types and the methods used to generate them.</summary>
    internal static IEnumerable<ICreateHandler> Handlers { get; } =
    [
        new FactoryCreateHandler<IFaked>(rand => (IFaked)rand.Options.Faker.Stub<object>().Dummy),
        new FactoryCreateHandler<ToolSet>(rand =>
            ToolSet.CreateViaSeed(rand.Options.Gen.Next<int>())
        ),
        new FactoryCreateHandler<SeededRandom>(rand => new SeededRandom(
            rand.Options.Gen.Next<int>()
        )),
        new FactoryCreateHandler<Limiter>(rand =>
            rand.Options.Gen.NextItem([Limiter.Few, Limiter.Dozen, Limiter.Score])
        ),
        new FactoryCreateHandler<IAsserter>(rand => rand.Create<ToolSet>().Asserter),
        new FactoryCreateHandler<IDuplicator>(rand => rand.Create<ToolSet>().Duplicator),
        new FactoryCreateHandler<IExtractor>(rand => rand.Create<ToolSet>().Extractor),
        new FactoryCreateHandler<IMutator>(rand => rand.Create<ToolSet>().Mutator),
        new FactoryCreateHandler<IRandomizer>(rand => rand.Create<ToolSet>().Randomizer),
        new FactoryCreateHandler<IRunner>(rand => rand.Create<ToolSet>().Runner),
        new FactoryCreateHandler<ITester>(rand => rand.Create<ToolSet>().Tester),
        new FactoryCreateHandler<IValuer>(rand => rand.Create<ToolSet>().Valuer),
        new FactoryCreateHandler<AsserterOptions>(CreateRandomOptionsBase<AsserterOptions>),
        new FactoryCreateHandler<DuplicatorOptions>(rand =>
            CreateRandomOptionsBase<DuplicatorOptions>(rand) with
            {
                Hints = [],
                NestedOptions = null,
                IncludeFoundHints = false,
                IncludeFrameworkHints = true,
                MaxHintRecursion = rand.Options.Gen.Next(28, 32),
            }
        ),
        new FactoryCreateHandler<ExtractorOptions>(rand =>
            CreateRandomOptionsBase<ExtractorOptions>(rand) with
            {
                Hints = [],
                NestedOptions = null,
                IncludeFoundHints = false,
                IncludeFrameworkHints = true,
                MaxHintRecursion = rand.Options.Gen.Next(28, 32),
            }
        ),
        new FactoryCreateHandler<MutatorOptions>(rand =>
            CreateRandomOptionsBase<MutatorOptions>(rand) with
            {
                Hints = [],
                NestedOptions = null,
                IncludeFoundHints = false,
                IncludeFrameworkHints = true,
                MaxHintRecursion = rand.Options.Gen.Next(28, 32),
            }
        ),
        new FactoryCreateHandler<RandomizerOptions>(rand =>
            CreateRandomOptionsBase<RandomizerOptions>(rand) with
            {
                Hints = [],
                NestedOptions = null,
                IncludeFoundHints = false,
                IncludeFrameworkHints = true,
                MaxHintRecursion = rand.Options.Gen.Next(28, 32),
                CollectionMinSize = rand.Options.Gen.Next(0, 1),
                CollectionMaxSize = rand.Options.Gen.Next(0, 4),
                StringMinSize = rand.Options.Gen.Next(0, 3),
                StringMaxSize = rand.Options.Gen.Next(0, 9),
            }
        ),
        new FactoryCreateHandler<RunnerOptions>(rand =>
            CreateRandomOptionsBase<RunnerOptions>(rand) with
            {
                Timeout = rand.Options.Gen.Next(new TimeSpan(0, 0, 5), new TimeSpan(0, 0, 10)),
            }
        ),
        new FactoryCreateHandler<TesterOptions>(CreateRandomOptionsBase<TesterOptions>),
        new FactoryCreateHandler<ValuerOptions>(rand =>
            CreateRandomOptionsBase<ValuerOptions>(rand) with
            {
                Hints = [],
                NestedOptions = null,
                IncludeFoundHints = false,
                IncludeFrameworkHints = true,
                MaxHintRecursion = rand.Options.Gen.Next(28, 32),
                AsyncTimeout = rand.Options.Gen.Next(new TimeSpan(0, 0, 50), new TimeSpan(0, 1, 0)),
            }
        ),
        new FactoryCreateHandler<AsserterMod>(rand => _ => rand.Create<AsserterOptions>()),
        new FactoryCreateHandler<DuplicatorMod>(rand => _ => rand.Create<DuplicatorOptions>()),
        new FactoryCreateHandler<ExtractorMod>(rand => _ => rand.Create<ExtractorOptions>()),
        new FactoryCreateHandler<MutatorMod>(rand => _ => rand.Create<MutatorOptions>()),
        new FactoryCreateHandler<RandomizerMod>(rand => _ => rand.Create<RandomizerOptions>()),
        new FactoryCreateHandler<RunnerMod>(rand => _ => rand.Create<RunnerOptions>()),
        new FactoryCreateHandler<TesterMod>(rand => _ => rand.Create<TesterOptions>()),
        new FactoryCreateHandler<ValuerMod>(rand => _ => rand.Create<ValuerOptions>()),
    ];

    private static T CreateRandomOptionsBase<T>(IRandomizerChainer randomizer)
    {
        return (T)_SubHint.TryCreate(typeof(T), randomizer).Data!;
    }
}
