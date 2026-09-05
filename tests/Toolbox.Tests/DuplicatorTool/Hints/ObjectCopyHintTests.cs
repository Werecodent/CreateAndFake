using Werecodent.CreateAndFake.DuplicatorTool.Hints;
using Werecodent.CreateAndFake.Samples.ErrorCases;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Hints;

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
