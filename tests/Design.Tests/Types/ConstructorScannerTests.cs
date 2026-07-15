using CreateAndFake.Design.Types;

namespace CreateAndFake.Design.Tests.Types;

#pragma warning disable IDE0051, MA0017, RCS1160, S3442 // For testing.

public static class ConstructorScannerTests
{
    internal abstract class BaseMembers
    {
        public BaseMembers() { }

        public BaseMembers(string _) { }
    }

    internal sealed class Members : BaseMembers
    {
        public Members()
            : base() { }

        public Members(int _)
            : base() { }

        internal Members(string text)
            : base(text) { }

        private Members(short _)
            : base() { }
    }

    [Fact]
    internal static Task ConstructorScanner_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ConstructorScanner>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task ConstructorScanner_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ConstructorScanner>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void All_ConstructorsFound()
    {
        new ConstructorScanner(typeof(Members)).All.Assert().HasCount(4);
    }

    [Fact]
    internal static void PublicOrInternal_ConstructorsFound()
    {
        ConstructorScanner scanner = new(typeof(Members));
        scanner.PublicOrInternal.Assert().HasCount(3);

        scanner.Visible.Assert().Is(scanner.PublicOrInternal);
    }

    [Fact]
    internal static void OnlyPublic_ConstructorsFound()
    {
        new ConstructorScanner(typeof(Members)).OnlyPublic.Assert().HasCount(2);
    }
}

#pragma warning restore
