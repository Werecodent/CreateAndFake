using System.Collections.Frozen;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Randomization;
using Werecodent.CreateAndFake.Design.Reiteration;
using Werecodent.CreateAndFake.DuplicatorTool;
using Werecodent.CreateAndFake.ExtractorTool;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.FakerTool.Proxy;
using Werecodent.CreateAndFake.MutatorTool;
using Werecodent.CreateAndFake.RandomizerTool.Engine;
using Werecodent.CreateAndFake.RandomizerTool.Hints;
using Werecodent.CreateAndFake.RunnerTool;
using Werecodent.CreateAndFake.TesterTool;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.RandomizerTool.Handlers;

internal static class SelfCreateHandlers
{
    private static readonly ObjectCreateHint _SubHint = new();

    /// <summary>Supported types and the methods used to generate them.</summary>
    internal static IEnumerable<ICreateHandler> Handlers { get; } =
    [
        new FactoryCreateHandler<SingleCallValueTaskSource>(rand => new SingleCallValueTaskSource(
            rand.Create<Guid>()
        )),
        new FactoryCreateHandler<ContentMap>(rand => new ContentMap(
            new HashSet<object>(rand.Create<IEnumerable<object>>(), rand.Options.Valuer),
            rand.Create<ExtractorOptions>()
        )),
        new FactoryCreateHandler<IFaked>(rand => (IFaked)rand.Options.Faker.Stub<object>().Dummy),
        new FactoryCreateHandler<ToolSet>(rand =>
            ToolSet.CreateViaSeed(rand.Options.Gen.Next<int>())
        ),
        new FactoryCreateHandler<SeededRandom>(rand => new SeededRandom(
            rand.Options.Gen.Next(7000, 8000),
            false,
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
        new FactoryCreateHandler<ConfiguredDesignOptions>(rand =>
            CreateRandomOptionsBase<ConfiguredDesignOptions>(rand) with
            {
                Seed = null,
                IterationLimit = rand.Options.Gen.Next(2900, 2960),
            }
        ),
        new FactoryCreateHandler<AsserterOptions>(rand =>
            CreateRandomOptionsBase<AsserterOptions>(rand) with
            {
                DisableCatchingForThrowAsserts = false,
            }
        ),
        new FactoryCreateHandler<DuplicatorOptions>(rand =>
            CreateRandomOptionsBase<DuplicatorOptions>(rand) with
            {
                Hints = [],
                NestedOptions = null,
                IncludeFoundHints = false,
                IncludeFrameworkHints = true,
                MaxRecursionDepth = rand.Options.Gen.Next(28, 32),
            }
        ),
        new FactoryCreateHandler<ExtractorOptions>(rand =>
            CreateRandomOptionsBase<ExtractorOptions>(rand) with
            {
                Hints = [],
                NestedOptions = null,
                IncludeFoundHints = false,
                IncludeFrameworkHints = true,
                ExtractPrivateMembers = false,
                MaxRecursionDepth = rand.Options.Gen.Next(28, 32),
            }
        ),
        new FactoryCreateHandler<FakerOptions>(rand =>
            CreateRandomOptionsBase<FakerOptions>(rand) with
            {
                Hints = [],
                NestedOptions = null,
                IncludeFoundHints = false,
                FakeDefaultGenerator = null,
                IncludeFrameworkHints = true,
                MaxRecursionDepth = rand.Options.Gen.Next(28, 32),
            }
        ),
        new FactoryCreateHandler<MutatorOptions>(rand =>
            CreateRandomOptionsBase<MutatorOptions>(rand) with
            {
                Hints = [],
                NestedOptions = null,
                IncludeFoundHints = false,
                IncludeFrameworkHints = true,
                MaxRecursionDepth = rand.Options.Gen.Next(28, 32),
            }
        ),
        new FactoryCreateHandler<RandomizerOptions>(rand =>
            CreateRandomOptionsBase<RandomizerOptions>(rand) with
            {
                Hints = [],
                NestedOptions = null,
                IncludeFoundHints = false,
                IncludeFrameworkHints = true,
                MaxRecursionDepth = rand.Options.Gen.Next(28, 32),
                CollectionMinSize = rand.Options.Gen.Next(0, 2),
                CollectionMaxSize = rand.Options.Gen.Next(1, 4),
                StringMinSize = rand.Options.Gen.Next(0, 3),
                StringMaxSize = rand.Options.Gen.Next(2, 9),
                FinalCondition = null,
                CreateCollectionAttemptLimit = rand.Options.Gen.NextItem([
                    Limiter.Dozen,
                    Limiter.Score,
                ]),
                StringCharacterSet = FrozenSet.ToFrozenSet(
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"
                ),
            }
        ),
        new FactoryCreateHandler<RunnerOptions>(rand =>
            CreateRandomOptionsBase<RunnerOptions>(rand) with
            {
                RunTimeout = rand.Options.Gen.Next(new TimeSpan(0, 0, 20), new TimeSpan(0, 0, 25)),
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
                MaxRecursionDepth = rand.Options.Gen.Next(28, 32),
                AsyncTimeout = rand.Options.Gen.Next(new TimeSpan(0, 0, 50), new TimeSpan(0, 1, 0)),
            }
        ),
        new FactoryCreateHandler<AsserterMod>(rand => _ => rand.Create<AsserterOptions>()),
        new FactoryCreateHandler<DuplicatorMod>(rand => _ => rand.Create<DuplicatorOptions>()),
        new FactoryCreateHandler<ExtractorMod>(rand => _ => rand.Create<ExtractorOptions>()),
        new FactoryCreateHandler<FakerMod>(rand => _ => rand.Create<FakerOptions>()),
        new FactoryCreateHandler<MutatorMod>(rand => _ => rand.Create<MutatorOptions>()),
        new FactoryCreateHandler<RandomizerMod>(rand => _ => rand.Create<RandomizerOptions>()),
        new FactoryCreateHandler<RunnerMod>(rand => _ => rand.Create<RunnerOptions>()),
        new FactoryCreateHandler<TesterMod>(rand => _ => rand.Create<TesterOptions>()),
        new FactoryCreateHandler<ValuerMod>(rand => _ => rand.Create<ValuerOptions>()),
        new FactoryCreateHandler<PlainAsyncDisposable>(rand => new PlainAsyncDisposable(
            rand.Create<string>()
        )),
    ];

    private static T CreateRandomOptionsBase<T>(IRandomizerChainer randomizer)
    {
        return (T)_SubHint.TryToCreate(typeof(T), randomizer).Data!;
    }
}
