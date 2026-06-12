using System.Reflection;
using CreateAndFake.AsserterTool;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.FakerTool.Proxy;
using CreateAndFake.Fluent.AssertCalls;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.AsserterTool;

public class AsserterTests
{
    private static readonly TesterMod config = opt =>
        opt with
        {
            IgnorableExceptions =
            [
                typeof(ArgumentException),
                typeof(AssertException),
                typeof(ToolException),
                typeof(FakeVerifyException),
                typeof(TimeoutException),
                typeof(UnsupportedException),
                typeof(TargetException),
                typeof(InvalidCastException),
            ],
        };

    private readonly AsserterMod _config;

    private bool _configCalled;

    public AsserterTests()
    {
        _configCalled = false;
        _config = opt =>
        {
            _configCalled = true;
            return opt;
        };
    }

    private readonly Asserter _testInstance = new(Tools.Asserter.Options);

    [Fact]
    internal static Task Asserter_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<Asserter>(
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static Task Asserter_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<Asserter>(
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static void Asserter_AllMethodsVirtual()
    {
        Tools.Asserter.IsEmpty(
            typeof(Asserter)
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(m => !m.IsVirtual)
                .Select(m => m.Name)
                .Where(n => n is not nameof(Asserter.Is) and not nameof(Asserter.IsNot))
                .Where(n => n is not $"get_{nameof(Asserter.Options)}")
        );
    }

    [Fact]
    internal void Fail_Throws()
    {
        _testInstance.Assert(t => t.Fail()).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal void Fail_ThrowsWithException(Exception error)
    {
        _testInstance
            .Assert(t => t.Fail(error))
            .Throws<AssertException>()
            .Exception.InnerException.Assert()
            .Is(error);
    }

    [Theory, RandomData]
    internal void Fail_ThrowsWithSample(DataSample sample)
    {
        sample.Assert(d => d.Assert().Fail()).Throws<AssertException>();
        sample.Assert(d => d.Assert().Fail()).Throws<AssertException>(_config);
        _configCalled.Assert().Is(true);
    }

    [Theory, RandomData]
    internal void Fail_OnlyThrows([Stub] IAsserter asserter, DataSample sample)
    {
        AssertObject instance = new(asserter, sample);
        instance.Fail();
        instance.Fail(_config);
    }

    [Theory, RandomData]
    internal void Pass_Works(DataSample sample)
    {
        sample.Assert().Pass();
        sample.Assert().Pass(_config);
        _configCalled.Assert().Is(true);
    }

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
}
