using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.DuplicatorTool.Engine;
using Werecodent.CreateAndFake.ExtractorTool;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.MutatorTool;
using Werecodent.CreateAndFake.RandomizerTool;
using Werecodent.CreateAndFake.RunnerTool;
using Werecodent.CreateAndFake.TesterTool;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.DuplicatorTool.Handlers;

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
