using CreateAndFake.Design.Extensions;

namespace CreateAndFake.Design.Tests.Extensions;

public static class TypeExtensionsTests
{
    [Fact]
    internal static Task TypeExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(
            typeof(TypeExtensions),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task TypeExtensions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(
            typeof(TypeExtensions),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void Inherits_StandardInheritanceWorks()
    {
        typeof(string).Inherits<IEnumerable<char>>().Assert().Is(true);
        typeof(IEnumerable<char>).Inherits<string>().Assert().Is(false);

        typeof(string).Inherits(typeof(IEnumerable<char>)).Assert().Is(true);
        typeof(IEnumerable<char>).Inherits(typeof(string)).Assert().Is(false);
    }

    [Fact]
    internal static void Inherits_GenericsWork()
    {
        typeof(IEnumerable<>).Inherits<string>().Assert().Is(false);
        typeof(List<>).Inherits<IList<int>>().Assert().Is(false);

        typeof(string).Inherits(typeof(IEnumerable<>)).Assert().Is(true);
        typeof(IEnumerable<>).Inherits(typeof(string)).Assert().Is(false);
        typeof(List<>).Inherits(typeof(IList<>)).Assert().Is(true);
        typeof(IList<>).Inherits(typeof(List<>)).Assert().Is(false);
        typeof(List<int>).Inherits(typeof(List<>)).Assert().Is(true);
        typeof(List<>).Inherits(typeof(List<int>)).Assert().Is(false);
    }

    [Theory, RandomData]
    internal static void Inherits_FalseWhenNull(Type type)
    {
        type.Inherits(null).Assert().Is(false);
        TypeExtensions.Inherits<string>(null).Assert().Is(false);
        TypeExtensions.Inherits(null, type).Assert().Is(false);
    }

    [Fact]
    internal static void IsInheritedBy_StandardInheritanceWorks()
    {
        typeof(string).IsInheritedBy<IEnumerable<char>>().Assert().Is(false);
        typeof(IEnumerable<char>).IsInheritedBy<string>().Assert().Is(true);

        typeof(string).IsInheritedBy(typeof(IEnumerable<char>)).Assert().Is(false);
        typeof(IEnumerable<char>).IsInheritedBy(typeof(string)).Assert().Is(true);
    }

    [Fact]
    internal static void IsInheritedBy_GenericsWork()
    {
        typeof(IEnumerable<>).IsInheritedBy<string>().Assert().Is(true);
        typeof(List<>).IsInheritedBy<IList<int>>().Assert().Is(false);

        typeof(string).IsInheritedBy(typeof(IEnumerable<>)).Assert().Is(false);
        typeof(IEnumerable<>).IsInheritedBy(typeof(string)).Assert().Is(true);
        typeof(List<>).IsInheritedBy(typeof(IList<>)).Assert().Is(false);
        typeof(IList<>).IsInheritedBy(typeof(List<>)).Assert().Is(true);
        typeof(List<int>).IsInheritedBy(typeof(List<>)).Assert().Is(false);
        typeof(List<>).IsInheritedBy(typeof(List<int>)).Assert().Is(true);
    }

    [Theory, RandomData]
    internal static void IsInheritedBy_FalseWhenNull(Type type)
    {
        type.IsInheritedBy(null).Assert().Is(false);
        TypeExtensions.IsInheritedBy<string>(null).Assert().Is(false);
        TypeExtensions.IsInheritedBy(null, type).Assert().Is(false);
    }
}
