namespace CreateAndFake.Design.Tests;

#pragma warning disable S3236 // For testing the methods.

public static class ArgumentGuardTests
{
    [Fact]
    public static Task ArgumentGuard_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(
            typeof(ArgumentGuard),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task ArgumentGuard_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(
            typeof(ArgumentGuard),
            TestContext.Current.CancellationToken
        );
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

#pragma warning restore S3236 // For testing the methods.
