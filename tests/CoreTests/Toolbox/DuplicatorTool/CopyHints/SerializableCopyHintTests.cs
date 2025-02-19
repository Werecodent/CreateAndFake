using CreateAndFake.Toolbox.DuplicatorTool.CopyHints;

namespace CreateAndFakeTests.Toolbox.DuplicatorTool.CopyHints;

public sealed class SerializableCopyHintTests : CopyHintTestBase<SerializableCopyHint>
{
    private static readonly Type[] _ValidTypes = [typeof(Exception), typeof(AggregateException)];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public SerializableCopyHintTests() : base(_ValidTypes, _InvalidTypes) { }
}
