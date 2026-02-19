using CreateAndFake.AsserterTool;
using CreateAndFake.DuplicatorTool.Engine;
using CreateAndFake.ExtractorTool;
using CreateAndFake.FakerTool;
using CreateAndFake.MutatorTool;
using CreateAndFake.RandomizerTool;
using CreateAndFake.RunnerTool;
using CreateAndFake.TesterTool;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.DuplicatorTool.Handlers;

/// <summary>Holds a collection of related handlers.</summary>
internal static class SelfCopyHandlers
{
    /// <summary>The collection of related handlers.</summary>
    internal static IEnumerable<ICopyHandler> Handlers { get; } =
    [
        new RefCopyHandler(typeof(AsserterOptions)),
        new RefCopyHandler(typeof(DuplicatorOptions)),
        new RefCopyHandler(typeof(ExtractorOptions)),
        new RefCopyHandler(typeof(FakerOptions)),
        new RefCopyHandler(typeof(MutatorOptions)),
        new RefCopyHandler(typeof(RandomizerOptions)),
        new RefCopyHandler(typeof(RunnerOptions)),
        new RefCopyHandler(typeof(TesterOptions)),
        new RefCopyHandler(typeof(ValuerOptions)),
        new RefCopyHandler(typeof(ToolSet)),
        new RefCopyHandler(typeof(Asserter)),
        new RefCopyHandler(typeof(Duplicator)),
        new RefCopyHandler(typeof(Extractor)),
        new RefCopyHandler(typeof(Faker)),
        new RefCopyHandler(typeof(Mutator)),
        new RefCopyHandler(typeof(Randomizer)),
        new RefCopyHandler(typeof(Runner)),
        new RefCopyHandler(typeof(Tester)),
        new RefCopyHandler(typeof(Valuer)),
    ];
}
