using System.Reflection;
using CreateAndFake.DuplicatorTool.Engine;
using CreateAndFake.DuplicatorTool.Hints;
using CreateAndFake.Samples.OldSamples;

namespace CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class BasicCopyHintTests : CopyHintTestBase<BasicCopyHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(BindingFlags),
        typeof(string),
        typeof(int),
        typeof(decimal),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public BasicCopyHintTests()
        : base(_ValidTypes, _InvalidTypes, true) { }

    [Fact]
    internal static void TryCopy_HandlesBaseObject()
    {
        object data = new();
        CopyHintResult result = new BasicCopyHint().TryCopy(data, CreateChainer());

        result.Assert().Is(new CopyHintResult(data));
        result.Data.Assert().ReferenceEqual(data);
    }
}
