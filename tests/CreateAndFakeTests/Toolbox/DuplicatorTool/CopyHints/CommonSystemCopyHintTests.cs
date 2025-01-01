using System.Reflection;
using CreateAndFake.Toolbox.DuplicatorTool.CopyHints;
using CreateAndFakeTests.TestBases;

namespace CreateAndFakeTests.Toolbox.DuplicatorTool.CopyHints;

public sealed class CommonSystemCopyHintTests : CopyHintTestBase<CommonSystemCopyHint>
{
    private static readonly Type[] _ValidTypes = [typeof(TimeSpan), typeof(Uri)];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public CommonSystemCopyHintTests() : base(_ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal static void TryCopy_HandlesMemberInfo(MemberInfo data)
    {
        (bool, object) result = new CommonSystemCopyHint().TryCopy(data, CreateChainer());

        result.Item1.Assert().Is(true);
        result.Item2.Assert().ReferenceEqual(data);
    }

    [Theory, RandomData]
    internal static void TryCopy_HandlesWeakReference(string data)
    {
        WeakReference original = new(data);

        (bool, object) result = new CommonSystemCopyHint().TryCopy(original, CreateChainer());

        result.Item1.Assert().Is(true);
        result.Item2.Assert().Is(original);
    }
}
