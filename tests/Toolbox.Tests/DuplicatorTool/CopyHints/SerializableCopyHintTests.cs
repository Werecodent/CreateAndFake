using System.Runtime.Serialization;
using CreateAndFake.DuplicatorTool.CopyHints;

namespace CreateAndFake.Tests.DuplicatorTool.CopyHints;

public sealed class SerializableCopyHintTests : CopyHintTestBase<SerializableCopyHint>
{
    private static readonly Type[] _ValidTypes = [typeof(Exception), typeof(AggregateException)];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public SerializableCopyHintTests() : base(_ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal void TryCopy_InvalidDataContractExceptionRethrown([Stub] ISerializable data)
    {
        TestInstance
            .Assert(t => t.TryCopy(data, CreateChainer()))
            .Throws<SerializationException>();
    }
}
