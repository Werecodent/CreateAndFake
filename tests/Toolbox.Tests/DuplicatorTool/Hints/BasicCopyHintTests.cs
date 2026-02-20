using System.Reflection;
using CreateAndFake.DuplicatorTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class BasicCopyHintTests : CopyHintTestBase<BasicCopyHint>
{
    private static readonly Type[] _ValidTypes = [typeof(BindingFlags), typeof(int)];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public BasicCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }
}
