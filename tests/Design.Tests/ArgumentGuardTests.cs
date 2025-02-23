namespace CreateAndFake.Design.Tests;

public static class ArgumentGuardTests
{
    [Fact]
    public static void ArgumentGuard_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException(typeof(ArgumentGuard));
    }

    [Fact]
    public static void ArgumentGuard_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation(typeof(ArgumentGuard));
    }

    [Theory, RandomData]
    internal static void ThrowIfNull_NoExceptionWithNonNull(object value, string name)
    {
        ArgumentGuard.ThrowIfNull(value, name);
    }

    [Theory, RandomData]
    internal static void ThrowIfNull_ExceptionWithNull(string name)
    {
        name.Assert(n => ArgumentGuard.ThrowIfNull(null, n)).Throws<ArgumentNullException>();
    }
}
