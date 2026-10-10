using System.Reflection;
using Werecodent.CreateAndFake.DuplicatorTool.Hints;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class BasicCopyHintTests : CopyHintTestBase<BasicCopyHint>
{
    private static readonly Type[] _ValidTypes = [typeof(BindingFlags), typeof(int)];

    public BasicCopyHintTests()
        : base(_ValidTypes) { }
}
