using CreateAndFake.AsserterTool;
using CreateAndFake.FakerTool;

namespace CreateAndFake.Tests.AsserterTool;

public sealed class AsserterDelegateTests
{
    private readonly Asserter _testInstance = new(Tools.Asserter.Options);

    [Fact]
    internal void CheckAll_RunsEachValidCase()
    {
        bool ran1 = false;
        bool ran2 = false;

        _testInstance.CheckAll(() => ran1 = true, () => ran2 = true);

        ran1.Assert().Is(true).Also(ran2).Is(true);
    }

    [Theory, RandomData]
    internal void CheckAll_SingleErrorThrows(Exception error)
    {
        bool ran2 = false;

        _testInstance
            .Assert(t => t.CheckAll(() => throw error, () => ran2 = true))
            .Throws<AggregateException>()
            .Exception.InnerExceptions.Assert()
            .Is(new[] { error })
            .Also(ran2)
            .Is(true);
    }

    [Theory, RandomData]
    internal void CheckAll_RunsEachErrorCase(Exception error1, Exception error2)
    {
        _testInstance
            .Assert(t => t.CheckAll(() => throw error1, () => throw error2))
            .Throws<AggregateException>()
            .Exception.InnerExceptions.Assert()
            .Is(new[] { error1, error2 });
    }

    [Fact]
    internal void Throws_ActionThrows()
    {
        _testInstance.Throws<InvalidOperationException>(() =>
            throw new InvalidOperationException()
        );
    }

    [Fact]
    internal void Throws_ActionTypeMismatch()
    {
        _testInstance
            .Assert(t => t.Throws<ArgumentException>(() => throw new NotSupportedException()))
            .Throws<AssertException>();
    }

    [Fact]
    internal void Throws_ActionNoThrow()
    {
        _testInstance
            .Assert(t => t.Throws<InvalidOperationException>(() => { }))
            .Throws<AssertException>();
    }

    [Fact]
    internal Task Throws_HandlesAsyncNoError()
    {
        return _testInstance.ThrowsAsync<InvalidDataException>(
            async () => await WaitTest(),
            TestContext.Current.CancellationToken
        );
    }

    private static async Task<bool> WaitTest()
    {
        await Task.Delay(0, TestContext.Current.CancellationToken);
        throw new InvalidDataException();
    }

    [Fact]
    internal void Throws_ActionNullCase()
    {
        _testInstance
            .Assert(t => t.Throws<InvalidOperationException>(null))
            .Throws<AssertException>();
    }

    [Fact]
    internal void Throws_FuncThrows()
    {
        _testInstance.Throws<InvalidOperationException>(() =>
            throw new InvalidOperationException()
        );
    }

    [Fact]
    internal void Throws_FuncTypeMismatch()
    {
        _testInstance
            .Assert(t => t.Throws<ArgumentException>(() => throw new NotSupportedException()))
            .Throws<AssertException>();
    }

    [Fact]
    internal void Throws_FuncNoThrow()
    {
        _testInstance
            .Assert(t => t.Throws<InvalidOperationException>(() => true))
            .Throws<AssertException>();
    }

    [Fact]
    internal void Throws_FuncNullCase()
    {
        _testInstance
            .Assert(t => t.Throws<InvalidOperationException>(null))
            .Throws<AssertException>();
    }

    [Theory, RandomData]
    internal void Throws_Disposes([Stub] IDisposable disposable)
    {
        disposable.ToFake().Setup(m => m.Dispose(), Behavior.None(Times.Once));

        _testInstance.Assert(t => t.Throws<Exception>(() => disposable)).Throws<AssertException>();

        disposable.Assert().Called();
    }

    [Theory, RandomData]
    internal void Throws_AggregateUnwraps(InvalidOperationException ex)
    {
        _testInstance
            .Throws<InvalidOperationException>(() => throw new AggregateException(ex))
            .Assert()
            .Is(ex);
    }

    [Theory, RandomData]
    internal void Throws_AggregateExtraInternal(InvalidOperationException error1, Exception error2)
    {
        AggregateException ex = new(error1, error2);

        _testInstance
            .Assert(t => t.Throws<InvalidOperationException>(() => throw ex))
            .Throws<AssertException>()
            .Exception.InnerException.Assert()
            .Is(ex);
    }

    [Theory, RandomData]
    internal void Throws_AggregateWrongInternals(InvalidOperationException error)
    {
        AggregateException ex = new(error);

        _testInstance
            .Assert(t => t.Throws<InvalidCastException>(() => throw ex))
            .Throws<AssertException>()
            .Exception.InnerException.Assert()
            .Is(ex);
    }
}
