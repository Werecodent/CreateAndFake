using System.Reflection;
using CreateAndFake.Design.Types;

namespace CreateAndFake.Design.Tests.Types;

public static class FactoryScannerTests
{
    private const BindingFlags _AllScope =
        BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

    internal abstract class BaseMembers
    {
        public static Members BaseInvalidMaker()
        {
            return null;
        }
    }

    internal sealed class Members : BaseMembers
    {
        private Members() { }

        public static Members PublicMaker()
        {
            return new();
        }

        public static Members PublicMakerWithParam(int _)
        {
            return new();
        }

        internal static Members InternalMaker()
        {
            return PrivateMaker();
        }

        private static Members PrivateMaker()
        {
            return new();
        }
    }

    [Fact]
    internal static Task FactoryScanner_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<FactoryScanner>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task FactoryScanner_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<FactoryScanner>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void All_FactoriesFound()
    {
        HashSet<MethodInfo> expectedFactories =
        [
            typeof(Members).GetMethod("PrivateMaker", _AllScope),
            typeof(Members).GetMethod(nameof(Members.PublicMaker), _AllScope),
            typeof(Members).GetMethod(nameof(Members.PublicMakerWithParam), _AllScope),
            typeof(Members).GetMethod(nameof(Members.InternalMaker), _AllScope),
        ];

        new FactoryScanner(typeof(Members)).All.ToHashSet().Assert().Is(expectedFactories);
    }

    [Fact]
    internal static void PublicOrInternal_FactoriesFound()
    {
        HashSet<MethodInfo> expectedFactories =
        [
            typeof(Members).GetMethod(nameof(Members.PublicMaker), _AllScope),
            typeof(Members).GetMethod(nameof(Members.PublicMakerWithParam), _AllScope),
            typeof(Members).GetMethod(nameof(Members.InternalMaker), _AllScope),
        ];

        FactoryScanner scanner = new(typeof(Members));
        scanner.PublicOrInternal.ToHashSet().Assert().Is(expectedFactories);

        scanner.Visible.Assert().Is(scanner.PublicOrInternal);
    }

    [Fact]
    internal static void OnlyPublic_FactoriesFound()
    {
        HashSet<MethodInfo> expectedFactories =
        [
            typeof(Members).GetMethod(nameof(Members.PublicMaker), _AllScope),
            typeof(Members).GetMethod(nameof(Members.PublicMakerWithParam), _AllScope),
        ];

        new FactoryScanner(typeof(Members)).OnlyPublic.ToHashSet().Assert().Is(expectedFactories);
    }
}
