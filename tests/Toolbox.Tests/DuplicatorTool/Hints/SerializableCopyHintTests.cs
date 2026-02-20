using System.Runtime.Serialization;
using CreateAndFake.DuplicatorTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class SerializableCopyHintTests : CopyHintTestBase<SerializableCopyHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(Exception),
        typeof(AggregateException),
        typeof(IOException),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public SerializableCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal void TryCopy_InvalidDataContractExceptionRethrown([Stub] ISerializable data)
    {
        TestInstance.Assert(t => t.TryCopy(data, CreateChainer())).Throws<SerializationException>();
    }
}
