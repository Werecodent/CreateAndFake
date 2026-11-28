using System.Collections;
using System.Reflection;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.RandomizerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class HandlerCreateHintTests : CreateHintTestBase<HandlerCreateHint>
{
    private static readonly Type[] _InvalidTypes =
    [
        typeof(IEnumerable),
        typeof(IEnumerable<>),
        typeof(IEnumerable<object>),
    ];

    public HandlerCreateHintTests()
        : base(null, _InvalidTypes) { }

    [Fact]
    internal void Debug_HandlerCreateHint_SupportedTypes()
    {
        TestInstance
            .SupportedTypes.OrderBy(t => t.Name)
            .Select(GenericConverter.ExpandName)
            .Assert()
            .Debug();
    }

    [Fact]
    internal void TryToCreate_ContinuesUntilMemberFound()
    {
        for (int i = 0; i < 50; i++)
        {
            _ = TestInstance.TryToCreate(typeof(FieldInfo), CreateChainer());
        }
    }
}
