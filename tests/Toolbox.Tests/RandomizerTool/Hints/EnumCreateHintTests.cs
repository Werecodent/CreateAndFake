using System.Reflection;
using Werecodent.CreateAndFake.RandomizerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class EnumCreateHintTests : CreateHintTestBase<EnumCreateHint>
{
    private static readonly Type[] _ValidTypes = [typeof(BindingFlags)];

    public EnumCreateHintTests()
        : base(_ValidTypes) { }
}
