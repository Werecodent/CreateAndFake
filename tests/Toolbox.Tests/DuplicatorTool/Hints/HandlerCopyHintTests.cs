using Werecodent.CreateAndFake.DuplicatorTool.Engine;
using Werecodent.CreateAndFake.DuplicatorTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class HandlerCopyHintTests : CopyHintTestBase<HandlerCopyHint>
{
    private static readonly Type[] _ValidTypes =
    [
        .. new HandlerCopyHint().SupportedTypes.Except([typeof(object)]),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public HandlerCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }

    /*[Theory, RandomData]
    internal static void TryCopy_HandlesWeakReference(string data)
    {
        WeakReference original = new(data);

        CopyHintResult result = new HandlerCopyHint().TryCopy(original, CreateChainer());

        result.HasData.Assert().Is(true);
        result.Data.Assert().Is(original);
    }*/

    [Fact]
    internal static void TryCopy_HandlesBaseObject()
    {
        object data = new();
        CopyHintResult result = new HandlerCopyHint().TryCopy(data, CreateChainer());

        result.Assert().Is(new CopyHintResult(data));
        result.Data.Assert().ReferenceEqual(data);
    }
}
