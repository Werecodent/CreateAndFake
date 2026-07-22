using CreateAndFake.Design.Exceptions;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Design.Tests;

public static class ArgumentGuardTests
{
    [Fact]
    public static Task ArgumentGuard_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(ArgumentGuard),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(IterationLimitException)] }
        );
    }

    [Fact]
    public static Task ArgumentGuard_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(ArgumentGuard),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(IterationLimitException)] }
        );
    }

    [Fact]
    public static Task IsAsynchronous_TrueWithUncompletedTask()
    {
        Task task = Task.Delay(100, TestContext.Current.CancellationToken);
        ArgumentGuard.IsAsynchronous(task).Assert().Is(true);
        ArgumentGuard.IsAsynchronous(Task.CompletedTask).Assert().Is(false);
        return task;
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
        Task task = Task.Delay(2000, TestContext.Current.CancellationToken);
        task.Assert(x => ArgumentGuard.ThrowIfAsynchronous(x, message))
            .Throws<AsynchronousAccessException>()
            .With.Message.Assert()
            .Contains(message);
    }

    [Theory, RandomData]
    public static void ThrowIfAsynchronous_TrueWithAsyncEnumerable(
        IAsyncEnumerable<int> data,
        string message
    )
    {
        data.Assert(x => ArgumentGuard.ThrowIfAsynchronous(x, message))
            .Throws<AsynchronousAccessException>()
            .With.Message.Assert()
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
    public static void ThrowUponIterationLimit_TestsLimit(int value)
    {
        value
            .Assert(x => ArgumentGuard.ThrowUponIterationLimit(x - 1, x))
            .ThrowsNo<IterationLimitException>();
        value
            .Assert(x => ArgumentGuard.ThrowUponIterationLimit(x, x))
            .Throws<IterationLimitException>();
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
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
    }

    [Theory, RandomData]
    internal static void ThrowIfNull_2_ExceptionWithNull(object nullValue, object value)
    {
        nullValue = null;
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(nullValue, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, nullValue))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
    }

    [Theory, RandomData]
    internal static void ThrowIfNull_3_ExceptionWithNull(object nullValue, object value)
    {
        nullValue = null;
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(nullValue, x, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, nullValue, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, x, nullValue))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
    }

    [Theory, RandomData]
    internal static void ThrowIfNull_4_ExceptionWithNull(object nullValue, object value)
    {
        nullValue = null;
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(nullValue, x, x, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, nullValue, x, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, x, nullValue, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, x, x, nullValue))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
    }

    [Theory, RandomData]
    internal static void ThrowIfNull_5_ExceptionWithNull(object nullValue, object value)
    {
        nullValue = null;
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(nullValue, x, x, x, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, nullValue, x, x, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, x, nullValue, x, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, x, x, nullValue, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, x, x, x, nullValue))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
    }

    [Theory, RandomData]
    internal static void ThrowIfNull_6_ExceptionWithNull(object nullValue, object value)
    {
        nullValue = null;
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(nullValue, x, x, x, x, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, nullValue, x, x, x, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, x, nullValue, x, x, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, x, x, nullValue, x, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, x, x, x, nullValue, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, x, x, x, x, nullValue))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
    }

    [Theory, RandomData]
    internal static void ThrowIfNull_7_ExceptionWithNull(object nullValue, object value)
    {
        nullValue = null;
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(nullValue, x, x, x, x, x, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, nullValue, x, x, x, x, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, x, nullValue, x, x, x, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, x, x, nullValue, x, x, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, x, x, x, nullValue, x, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, x, x, x, x, nullValue, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, x, x, x, x, x, nullValue))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
    }

    [Theory, RandomData]
    internal static void ThrowIfNull_8_ExceptionWithNull(object nullValue, object value)
    {
        nullValue = null;
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(nullValue, x, x, x, x, x, x, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, nullValue, x, x, x, x, x, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, x, nullValue, x, x, x, x, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, x, x, nullValue, x, x, x, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, x, x, x, nullValue, x, x, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, x, x, x, x, nullValue, x, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, x, x, x, x, x, nullValue, x))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
        value
            .Assert(x => ArgumentGuard.ThrowIfNull(x, x, x, x, x, x, x, nullValue))
            .Throws<ArgumentNullException>()
            .With.ParamName.Assert()
            .Is(nameof(nullValue));
    }
}
