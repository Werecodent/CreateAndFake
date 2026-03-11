using System.Reflection;
using CreateAndFake.Design.Types;

namespace CreateAndFake.Design.Tests.Types;

#pragma warning disable CA1823, CS0169, CS0414, IDE0044, IDE0051, RCS1169, RCS1213, S1144, S2933 // For testing.

public static class FieldScannerTests
{
    private const BindingFlags _AllScope =
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    [Fact]
    internal static Task FieldScanner_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<FieldScanner>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task FieldScanner_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<FieldScanner>(
            TestContext.Current.CancellationToken
        );
    }

    private abstract class BaseMemberHolder
    {
        public static int _StaticBaseField = 0;

        private int _privateBaseMutateField = 0;

        private readonly int _privateBaseField = 0;

        internal int _internalBaseMutateField = 0;

        internal readonly int _internalBaseField = 0;

        public int _publicBaseMutateField = 0;

        public readonly int _publicBaseField = 0;
    }

    private sealed class MemberHolder : BaseMemberHolder
    {
        public static int _StaticField = 0;

        private int _privateMutateField = 0;

        private readonly int _privateField = 0;

        internal int _internalMutateField = 0;

        internal readonly int _internalField = 0;

        public int _publicMutateField = 0;

        public readonly int _publicField = 0;
    }

    [Fact]
    internal static void All_FieldsFound()
    {
        HashSet<FieldInfo> expectedFields =
        [
            typeof(BaseMemberHolder).GetField("_privateBaseMutateField", _AllScope),
            typeof(BaseMemberHolder).GetField("_privateBaseField", _AllScope),
            typeof(MemberHolder).GetField(nameof(MemberHolder._internalBaseField), _AllScope),
            typeof(MemberHolder).GetField(nameof(MemberHolder._internalBaseMutateField), _AllScope),
            typeof(MemberHolder).GetField(nameof(MemberHolder._publicBaseField), _AllScope),
            typeof(MemberHolder).GetField(nameof(MemberHolder._publicBaseMutateField), _AllScope),
            typeof(MemberHolder).GetField("_privateField", _AllScope),
            typeof(MemberHolder).GetField("_privateMutateField", _AllScope),
            typeof(MemberHolder).GetField(nameof(MemberHolder._internalField), _AllScope),
            typeof(MemberHolder).GetField(nameof(MemberHolder._internalMutateField), _AllScope),
            typeof(MemberHolder).GetField(nameof(MemberHolder._publicField), _AllScope),
            typeof(MemberHolder).GetField(nameof(MemberHolder._publicMutateField), _AllScope),
        ];

        new FieldScanner(typeof(MemberHolder)).All.Assert().Is(expectedFields);
    }

    [Fact]
    internal static void Visible_FieldsFound()
    {
        HashSet<FieldInfo> expectedFields =
        [
            typeof(MemberHolder).GetField(nameof(MemberHolder._internalBaseField), _AllScope),
            typeof(MemberHolder).GetField(nameof(MemberHolder._internalBaseMutateField), _AllScope),
            typeof(MemberHolder).GetField(nameof(MemberHolder._publicBaseField), _AllScope),
            typeof(MemberHolder).GetField(nameof(MemberHolder._publicBaseMutateField), _AllScope),
            typeof(MemberHolder).GetField(nameof(MemberHolder._internalField), _AllScope),
            typeof(MemberHolder).GetField(nameof(MemberHolder._internalMutateField), _AllScope),
            typeof(MemberHolder).GetField(nameof(MemberHolder._publicField), _AllScope),
            typeof(MemberHolder).GetField(nameof(MemberHolder._publicMutateField), _AllScope),
        ];

        new FieldScanner(typeof(MemberHolder)).Visible.ToHashSet().Assert().Is(expectedFields);
    }

    [Fact]
    internal static void Writable_FieldsFound()
    {
        HashSet<FieldInfo> expectedFields =
        [
            typeof(MemberHolder).GetField(nameof(MemberHolder._internalBaseMutateField), _AllScope),
            typeof(MemberHolder).GetField(nameof(MemberHolder._publicBaseMutateField), _AllScope),
            typeof(MemberHolder).GetField(nameof(MemberHolder._internalMutateField), _AllScope),
            typeof(MemberHolder).GetField(nameof(MemberHolder._publicMutateField), _AllScope),
        ];

        new FieldScanner(typeof(MemberHolder)).Writable.ToHashSet().Assert().Is(expectedFields);
    }

    [Fact]
    internal static void Public_FieldsFound()
    {
        HashSet<FieldInfo> expectedFields =
        [
            typeof(MemberHolder).GetField(nameof(MemberHolder._publicBaseField), _AllScope),
            typeof(MemberHolder).GetField(nameof(MemberHolder._publicBaseMutateField), _AllScope),
            typeof(MemberHolder).GetField(nameof(MemberHolder._publicField), _AllScope),
            typeof(MemberHolder).GetField(nameof(MemberHolder._publicMutateField), _AllScope),
        ];

        new FieldScanner(typeof(MemberHolder)).OnlyPublic.ToHashSet().Assert().Is(expectedFields);
    }
}

#pragma warning restore CA1823, CS0169, CS0414, IDE0044, IDE0051, RCS1169, RCS1213, S1144, S2933
