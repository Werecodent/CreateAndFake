using System.Reflection;
using CreateAndFake.Design.Types;

namespace CreateAndFake.Design.Tests.Types;

#pragma warning disable CS0628, IDE0051, CA1822, RCS1213, S1144, S1186 // For testing.

public static class MethodScannerTests
{
    private const BindingFlags _AllScope =
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    internal abstract class BaseMembers
    {
        public static void BaseStaticMethod() { }

        public void BasePublicMethod() { }

        protected void BaseProtectedMethod() { }

        protected internal void BaseProtectedInternalMethod() { }

        internal void BaseInternalMethod() { }

        private void BasePrivateMethod() { }
    }

    internal sealed class Members : BaseMembers
    {
        public static void StaticMethod() { }

        public void PublicMethod() { }

        protected void ProtectedMethod() { }

        protected internal void ProtectedInternalMethod() { }

        internal void InternalMethod() { }

        private void PrivateMethod() { }
    }

    [Fact]
    internal static Task MethodScanner_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<MethodScanner>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task MethodScanner_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<MethodScanner>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void All_MethodsFound()
    {
        HashSet<MethodInfo> expectedMethods =
        [
            typeof(BaseMembers).GetMethod("BasePrivateMethod", _AllScope),
            typeof(Members).GetMethod("BaseProtectedMethod", _AllScope),
            typeof(Members).GetMethod("PrivateMethod", _AllScope),
            typeof(Members).GetMethod("ProtectedMethod", _AllScope),
            typeof(Members).GetMethod("Finalize", _AllScope),
            typeof(Members).GetMethod(nameof(Members.BasePublicMethod), _AllScope),
            typeof(Members).GetMethod(nameof(Members.BaseProtectedInternalMethod), _AllScope),
            typeof(Members).GetMethod(nameof(Members.BaseInternalMethod), _AllScope),
            typeof(Members).GetMethod(nameof(Members.PublicMethod), _AllScope),
            typeof(Members).GetMethod(nameof(Members.ProtectedInternalMethod), _AllScope),
            typeof(Members).GetMethod(nameof(Members.InternalMethod), _AllScope),
            typeof(Members).GetMethod(nameof(MemberwiseClone), _AllScope),
            typeof(Members).GetMethod(nameof(GetType)),
            typeof(Members).GetMethod(nameof(ToString)),
            typeof(Members).GetMethod(nameof(Equals)),
            typeof(Members).GetMethod(nameof(GetHashCode)),
        ];

        new MethodScanner(typeof(Members)).All.ToHashSet().Assert().Is(expectedMethods);
    }

    [Fact]
    internal static void PublicOrInternal_MethodsFound()
    {
        HashSet<MethodInfo> expectedMethods =
        [
            typeof(Members).GetMethod(nameof(Members.BasePublicMethod), _AllScope),
            typeof(Members).GetMethod(nameof(Members.BaseProtectedInternalMethod), _AllScope),
            typeof(Members).GetMethod(nameof(Members.BaseInternalMethod), _AllScope),
            typeof(Members).GetMethod(nameof(Members.PublicMethod), _AllScope),
            typeof(Members).GetMethod(nameof(Members.ProtectedInternalMethod), _AllScope),
            typeof(Members).GetMethod(nameof(Members.InternalMethod), _AllScope),
            typeof(Members).GetMethod(nameof(MemberwiseClone), _AllScope),
            typeof(Members).GetMethod(nameof(GetType)),
            typeof(Members).GetMethod(nameof(ToString)),
            typeof(Members).GetMethod(nameof(Equals)),
            typeof(Members).GetMethod(nameof(GetHashCode)),
        ];

        MethodScanner scanner = new(typeof(Members));
        scanner.PublicOrInternal.ToHashSet().Assert().Is(expectedMethods);

        scanner.Visible.Assert().Is(scanner.PublicOrInternal);
    }

    [Fact]
    internal static void OnlyPublic_MethodsFound()
    {
        HashSet<MethodInfo> expectedMethods =
        [
            typeof(Members).GetMethod(nameof(Members.BasePublicMethod), _AllScope),
            typeof(Members).GetMethod(nameof(Members.PublicMethod), _AllScope),
            typeof(Members).GetMethod(nameof(GetType)),
            typeof(Members).GetMethod(nameof(ToString)),
            typeof(Members).GetMethod(nameof(Equals)),
            typeof(Members).GetMethod(nameof(GetHashCode)),
        ];

        new MethodScanner(typeof(Members)).OnlyPublic.ToHashSet().Assert().Is(expectedMethods);
    }
}

#pragma warning restore
