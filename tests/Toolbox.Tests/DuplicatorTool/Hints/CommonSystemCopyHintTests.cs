using System.Reflection;
using CreateAndFake.DuplicatorTool.Engine;
using CreateAndFake.DuplicatorTool.Hints;

namespace CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class CommonSystemCopyHintTests : CopyHintTestBase<CommonSystemCopyHint>
{
    private static readonly Type[] _ValidTypes = [typeof(TimeSpan), typeof(Uri)];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public CommonSystemCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal static void TryCopy_HandlesMemberInfo(MemberInfo data)
    {
        CopyHintResult result = new CommonSystemCopyHint().TryCopy(data, CreateChainer());

        result.HasData.Assert().Is(true);
        result.Data.Assert().ReferenceEqual(data);
    }

    [Theory, RandomData]
    internal static void TryCopy_HandlesWeakReference(string data)
    {
        WeakReference original = new(data);

        CopyHintResult result = new CommonSystemCopyHint().TryCopy(original, CreateChainer());

        result.HasData.Assert().Is(true);
        result.Data.Assert().Is(original);
    }
}
