using CreateAndFake.DuplicatorTool.Hints;
using CreateAndFake.Samples.ErrorCases;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class ObjectCopyHintTests : CopyHintTestBase<ObjectCopyHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(PrivateValuerEquatableSample),
        typeof(IUnimplementedSample),
        typeof(DataHolderSample),
        typeof(FieldSample),
        typeof(DataHolderSample),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(MismatchDataSample)];

    public ObjectCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }

    public override Task CopyHint_NoParameterMutation()
    {
        return Task.CompletedTask;
    }
}
