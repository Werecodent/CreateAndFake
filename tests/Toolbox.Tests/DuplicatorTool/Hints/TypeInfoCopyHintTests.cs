using System.Reflection;
using CreateAndFake.DuplicatorTool.Engine;
using CreateAndFake.DuplicatorTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class TypeInfoCopyHintTests : CopyHintTestBase<TypeInfoCopyHint>
{
    private static readonly Type[] _ValidTypes = [];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public TypeInfoCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal static void TryCopy_HandlesMemberInfo(MemberInfo data)
    {
        // typeof(MemberInfo), typeof(MethodBase), typeof(ParameterInfo), typeof(Type),
        // typeof(NumberFormatInfo), typeof(DateTimeFormatInfo), typeof(CultureInfo)
        CopyHintResult result = new TypeInfoCopyHint().TryCopy(data, CreateChainer());

        result.HasData.Assert().Is(true);
        result.Data.Assert().ReferenceEqual(data);
    }
}
