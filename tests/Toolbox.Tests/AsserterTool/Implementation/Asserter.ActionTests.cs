using CreateAndFake.AsserterTool;
using CreateAndFake.FakerTool;

namespace CreateAndFake.Tests.AsserterTool.Implementation;

public sealed class AsserterActionTests
{
    private readonly Asserter _testInstance = new(Tools.Asserter.Options);

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
            .Assert(x => x.Throws<ArgumentException>(() => throw new NotSupportedException()))
            .Throws<AssertException>();
    }

    [Fact]
    internal void Throws_ActionNoThrow()
    {
        _testInstance
            .Assert(x => x.Throws<InvalidOperationException>(() => { }))
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
            .Assert(x => x.Throws<InvalidOperationException>(null))
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
            .Assert(x => x.Throws<ArgumentException>(() => throw new NotSupportedException()))
            .Throws<AssertException>();
    }

    [Fact]
    internal void Throws_FuncNoThrow()
    {
        _testInstance
            .Assert(x => x.Throws<InvalidOperationException>(() => true))
            .Throws<AssertException>();
    }

    [Fact]
    internal void Throws_FuncNullCase()
    {
        _testInstance
            .Assert(x => x.Throws<InvalidOperationException>(null))
            .Throws<AssertException>();
    }

    [Theory, RandomData]
    internal void Throws_Disposes([Stub] IDisposable disposable)
    {
        disposable.SetupReturn(d => d.Dispose(), Behavior.None(Times.Once));

        _testInstance.Assert(x => x.Throws<Exception>(() => disposable)).Throws<AssertException>();

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
            .Assert(x => x.Throws<InvalidOperationException>(() => throw ex))
            .Throws<AssertException>()
            .With.InnerException.Assert()
            .Is(ex);
    }

    [Theory, RandomData]
    internal void Throws_AggregateWrongInternals(InvalidOperationException error)
    {
        AggregateException ex = new(error);

        _testInstance
            .Assert(x => x.Throws<InvalidCastException>(() => throw ex))
            .Throws<AssertException>()
            .With.InnerException.Assert()
            .Is(ex);
    }
}
