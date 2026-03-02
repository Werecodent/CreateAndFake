using System.Reflection;
using CreateAndFake.AsserterTool;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.FakerTool.Proxy;

namespace CreateAndFake.Tests.AsserterTool;

public static class AsserterTests
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

    private static readonly Asserter _testInstance = new(Tools.Asserter.Options);

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
    internal static void Fail_Throws()
    {
        _testInstance.Assert(t => t.Fail()).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void Fail_ThrowsWithException(Exception error)
    {
        _testInstance
            .Assert(t => t.Fail(error))
            .Throws<AssertException>()
            .InnerException.Assert()
            .Is(error);
    }
}
