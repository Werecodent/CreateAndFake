using CreateAndFake.DuplicatorTool.Engine;
using CreateAndFake.DuplicatorTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class HandlerCopyHintTests : CopyHintTestBase<HandlerCopyHint>
{
    private static readonly Type[] _ValidTypes = [typeof(Guid), typeof(TimeSpan), typeof(Uri)];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public HandlerCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal static void TryCopy_HandlesWeakReference(string data)
    {
        WeakReference original = new(data);

        CopyHintResult result = new HandlerCopyHint().TryCopy(original, CreateChainer());

        result.HasData.Assert().Is(true);
        result.Data.Assert().Is(original);
    }
}
