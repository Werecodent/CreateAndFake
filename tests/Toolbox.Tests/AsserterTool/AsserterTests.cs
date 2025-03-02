using System.Reflection;
using CreateAndFake.AsserterTool;

namespace CreateAndFake.Tests.AsserterTool;

public static class AsserterTests
{
    private static readonly Asserter _testInstance = new(Tools.Asserter.Options);

    [Fact]
    internal static void Asserter_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<Asserter>();
    }

    [Fact]
    internal static void Asserter_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<Asserter>();
    }

    [Fact]
    internal static void Asserter_AllMethodsVirtual()
    {
        Tools.Asserter.IsEmpty(typeof(Asserter)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Where(m => !m.IsVirtual)
            .Select(m => m.Name)
            .Where(n => n is not nameof(Asserter.Is) and not nameof(Asserter.IsNot))
            .Where(n => n is not $"get_{nameof(Asserter.Options)}"));
    }

    [Fact]
    internal static void Fail_Throws()
    {
        _testInstance.Assert(t => t.Fail()).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void Fail_ThrowsWithException(Exception error)
    {
        _testInstance
            .Assert(t => t.Fail(error))
            .Throws<AssertException>().InnerException.Assert()
            .Is(error);
    }
}
