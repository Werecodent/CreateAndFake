using CreateAndFake.DuplicatorTool.Engine;
using CreateAndFake.DuplicatorTool.Hints;

namespace CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class CopierCopyHintTests : CopyHintTestBase<CopierCopyHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(Guid),
        typeof(IntPtr),
        typeof(TimeSpan),
        typeof(Uri),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public CopierCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal static void TryCopy_HandlesWeakReference(string data)
    {
        WeakReference original = new(data);

        CopyHintResult result = new CopierCopyHint().TryCopy(original, CreateChainer());

        result.HasData.Assert().Is(true);
        result.Data.Assert().Is(original);
    }
}
