using System.Reflection;
using CreateAndFake.Design.Types;

namespace CreateAndFake.Design.Tests.Types;

#pragma warning disable CS0628, IDE0051, CA1822, RCS1213, S1144, S1186 // For testing.

public static class StaticMethodScannerTests
{
    private const BindingFlags _AllScope =
        BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

    internal abstract class BaseMembers
    {
        public void BasePublicMethod() { }

        public static void BaseStaticMethod() { }
    }

    internal sealed class Members : BaseMembers
    {
        public void PublicMethod() { }

        public static void StaticMethod() { }

        protected static void ProtectedStaticMethod() { }

        protected internal static void ProtectedInternalStaticMethod() { }

        internal static void InternalStaticMethod() { }

        private static void PrivateStaticMethod() { }
    }

    [Fact]
    internal static Task StaticMethodScanner_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<StaticMethodScanner>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task StaticMethodScanner_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<StaticMethodScanner>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void All_StaticMethodsFound()
    {
        HashSet<MethodInfo> expectedMethods =
        [
            typeof(Members).GetMethod("PrivateStaticMethod", _AllScope),
            typeof(Members).GetMethod("ProtectedStaticMethod", _AllScope),
            typeof(Members).GetMethod(nameof(Members.StaticMethod), _AllScope),
            typeof(Members).GetMethod(nameof(Members.ProtectedInternalStaticMethod), _AllScope),
            typeof(Members).GetMethod(nameof(Members.InternalStaticMethod), _AllScope),
        ];

        new StaticMethodScanner(typeof(Members)).All.ToHashSet().Assert().Is(expectedMethods);
    }

    [Fact]
    internal static void PublicOrInternal_StaticMethodsFound()
    {
        HashSet<MethodInfo> expectedMethods =
        [
            typeof(Members).GetMethod(nameof(Members.StaticMethod), _AllScope),
            typeof(Members).GetMethod(nameof(Members.ProtectedInternalStaticMethod), _AllScope),
            typeof(Members).GetMethod(nameof(Members.InternalStaticMethod), _AllScope),
        ];

        StaticMethodScanner scanner = new(typeof(Members));
        scanner
            .PublicOrInternal.ToHashSet()
            .Assert()
            .Is(expectedMethods)
            .And.Is(scanner.Visible.ToHashSet());
    }

    [Fact]
    internal static void OnlyPublic_StaticMethodsFound()
    {
        HashSet<MethodInfo> expectedMethods =
        [
            typeof(Members).GetMethod(nameof(Members.StaticMethod), _AllScope),
        ];

        new StaticMethodScanner(typeof(Members))
            .OnlyPublic.ToHashSet()
            .Assert()
            .Is(expectedMethods);
    }
}

#pragma warning restore
