using CreateAndFake.Design.Exceptions;
using CreateAndFake.Samples.Scenarios;

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

    [Fact]
    public static void IsAsynchronous_TrueWithTask()
    {
        Task task = Task.Delay(100, TestContext.Current.CancellationToken);
        ArgumentGuard.IsAsynchronous(task).Assert().Is(true);
    }

    [Theory, RandomData]
    public static void IsAsynchronous_TrueWithAsyncEnumerable(IAsyncEnumerable<int> data)
    {
        ArgumentGuard.IsAsynchronous(data).Assert().Is(true);
    }

    [Fact]
    public static void IsAsynchronous_FalseWithNull()
    {
        ArgumentGuard.IsAsynchronous(null).Assert().Is(false);
    }

    [Theory, RandomData]
    public static void IsAsynchronous_FalseWithSyncData(DataSample data)
    {
        ArgumentGuard.IsAsynchronous(data).Assert().Is(false);
    }

    [Theory, RandomData]
    public static void ThrowIfAsynchronous_TrueWithTask(string message)
    {
        Task task = Task.Delay(100, TestContext.Current.CancellationToken);
        task.Assert(t => ArgumentGuard.ThrowIfAsynchronous(t, message))
            .Throws<EngineException>()
            .Message.Assert()
            .Contains(message);
    }

    [Theory, RandomData]
    public static void ThrowIfAsynchronous_TrueWithAsyncEnumerable(
        IAsyncEnumerable<int> data,
        string message
    )
    {
        data.Assert(d => ArgumentGuard.ThrowIfAsynchronous(d, message))
            .Throws<EngineException>()
            .Message.Assert()
            .Contains(message);
    }

    [Theory, RandomData]
    public static void ThrowIfAsynchronous_PassWithNull(string message)
    {
        ArgumentGuard.ThrowIfAsynchronous(null, message);
    }

    [Theory, RandomData]
    public static void ThrowIfAsynchronous_PassWithSyncData(DataSample data, string message)
    {
        ArgumentGuard.ThrowIfAsynchronous(data, message);
    }

    [Theory, RandomData]
    internal static void ThrowIfNull_NoExceptionWithNonNull(object value)
    {
        ArgumentGuard.ThrowIfNull(value);
        ArgumentGuard.ThrowIfNull(value, value);
        ArgumentGuard.ThrowIfNull(value, value, value);
        ArgumentGuard.ThrowIfNull(value, value, value, value);
        ArgumentGuard.ThrowIfNull(value, value, value, value, value);
        ArgumentGuard.ThrowIfNull(value, value, value, value, value, value);
        ArgumentGuard.ThrowIfNull(value, value, value, value, value, value, value);
        ArgumentGuard.ThrowIfNull(value, value, value, value, value, value, value, value);
    }

    [Theory, RandomData]
    internal static void ThrowIfNull_1_ExceptionWithNull(object nullValue, object value)
    {
        nullValue = null;
        value
            .Assert(_ => ArgumentGuard.ThrowIfNull(nullValue))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
    }

    [Theory, RandomData]
    internal static void ThrowIfNull_2_ExceptionWithNull(object nullValue, object value)
    {
        nullValue = null;
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(nullValue, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, nullValue))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
    }

    [Theory, RandomData]
    internal static void ThrowIfNull_3_ExceptionWithNull(object nullValue, object value)
    {
        nullValue = null;
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(nullValue, v, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, nullValue, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, v, nullValue))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
    }

    [Theory, RandomData]
    internal static void ThrowIfNull_4_ExceptionWithNull(object nullValue, object value)
    {
        nullValue = null;
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(nullValue, v, v, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, nullValue, v, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, v, nullValue, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, v, v, nullValue))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
    }

    [Theory, RandomData]
    internal static void ThrowIfNull_5_ExceptionWithNull(object nullValue, object value)
    {
        nullValue = null;
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(nullValue, v, v, v, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, nullValue, v, v, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, v, nullValue, v, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, v, v, nullValue, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, v, v, v, nullValue))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
    }

    [Theory, RandomData]
    internal static void ThrowIfNull_6_ExceptionWithNull(object nullValue, object value)
    {
        nullValue = null;
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(nullValue, v, v, v, v, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, nullValue, v, v, v, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, v, nullValue, v, v, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, v, v, nullValue, v, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, v, v, v, nullValue, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, v, v, v, v, nullValue))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
    }

    [Theory, RandomData]
    internal static void ThrowIfNull_7_ExceptionWithNull(object nullValue, object value)
    {
        nullValue = null;
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(nullValue, v, v, v, v, v, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, nullValue, v, v, v, v, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, v, nullValue, v, v, v, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, v, v, nullValue, v, v, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, v, v, v, nullValue, v, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, v, v, v, v, nullValue, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, v, v, v, v, v, nullValue))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
    }

    [Theory, RandomData]
    internal static void ThrowIfNull_8_ExceptionWithNull(object nullValue, object value)
    {
        nullValue = null;
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(nullValue, v, v, v, v, v, v, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, nullValue, v, v, v, v, v, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, v, nullValue, v, v, v, v, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, v, v, nullValue, v, v, v, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, v, v, v, nullValue, v, v, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, v, v, v, v, nullValue, v, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, v, v, v, v, v, nullValue, v))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(v => ArgumentGuard.ThrowIfNull(v, v, v, v, v, v, v, nullValue))
            .Throws<ArgumentNullException>()
            .ParamName.Assert()
            .Is(nameof(nullValue));
    }
}

#pragma warning restore S3236 // For testing the methods.
