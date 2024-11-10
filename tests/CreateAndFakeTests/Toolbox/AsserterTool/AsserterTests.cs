using System.Reflection;
using CreateAndFake.Toolbox.AsserterTool;
using CreateAndFake.Toolbox.FakerTool;

namespace CreateAndFakeTests.Toolbox.AsserterTool;

public sealed class AsserterTests
{
    private readonly Asserter _testInstance;

    public AsserterTests()
    {
        _testInstance = new Asserter(Tools.Asserter.Options);
    }

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
            .Assert(t => t.CheckAll(
                () => throw error,
                () => ran2 = true))
            .Throws<AggregateException>().InnerExceptions
            .Assert().Is(new[] { error })
            .Also(ran2).Is(true);
    }

    [Theory, RandomData]
    internal void CheckAll_RunsEachErrorCase(Exception error1, Exception error2)
    {
        _testInstance
            .Assert(t => t.CheckAll(
                () => throw error1,
                () => throw error2))
            .Throws<AggregateException>().InnerExceptions
            .Assert().Is(new[] { error1, error2 });
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
            .Throws<AssertException>().InnerException.Assert()
            .Is(error);
    }

    [Fact]
    internal void Throws_ActionThrows()
    {
        _testInstance.Throws<InvalidOperationException>((Action)(() => throw new InvalidOperationException()));
    }

    [Fact]
    internal void Throws_ActionTypeMismatch()
    {
        _testInstance
            .Assert(t => t.Throws<ArgumentException>((Action)(() => throw new NotImplementedException())))
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
    internal void Throws_ActionNullCase()
    {
        _testInstance
            .Assert(t => t.Throws<InvalidOperationException>((Action)null))
            .Throws<AssertException>();
    }

    [Fact]
    internal void Throws_FuncThrows()
    {
        _testInstance.Throws<InvalidOperationException>(() => throw new InvalidOperationException());
    }

    [Fact]
    internal void Throws_FuncTypeMismatch()
    {
        _testInstance
            .Assert(t => t.Throws<ArgumentException>(() => throw new NotImplementedException()))
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

        _testInstance
            .Assert(t => t.Throws<Exception>(() => disposable))
            .Throws<AssertException>();

        disposable.VerifyAllCalls();
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
            .Throws<AssertException>().InnerException
            .Assert().Is(ex);
    }

    [Theory, RandomData]
    internal void Throws_AggregateWrongInternals(InvalidOperationException error)
    {
        AggregateException ex = new(error);

        _testInstance
            .Assert(t => t.Throws<InvalidCastException>(() => throw ex))
            .Throws<AssertException>().InnerException
            .Assert().Is(ex);
    }

    [Theory, RandomData]
    internal void IsEmpty_Works(IEnumerable<string> data)
    {
        _testInstance.IsEmpty(Array.Empty<string>());

        _testInstance.Assert(t => t.IsEmpty(null)).Throws<AssertException>();
        _testInstance.Assert(t => t.IsEmpty(data)).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal void IsNotEmpty_Works(IEnumerable<string> data)
    {
        _testInstance.IsNotEmpty(data);

        _testInstance.Assert(t => t.IsNotEmpty(null)).Throws<AssertException>();
        _testInstance.Assert(t => t.IsNotEmpty(Array.Empty<string>())).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal void HasCount_Works(IEnumerable<string> data)
    {
        _testInstance.HasCount(data.Count(), data);

        _testInstance.Assert(t => t.HasCount(data.Count(), null)).Throws<AssertException>();
        _testInstance.Assert(t => t.HasCount(data.Count() - 1, data)).Throws<AssertException>();
        _testInstance.Assert(t => t.HasCount(data.Count() + 1, data)).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal void ReferenceEqual_NotByValue([Stub] object fake)
    {
        fake.Equals(Arg.Any<object>()).SetupReturn(true, Times.Never);

        _testInstance.ReferenceEqual(fake, fake);
        _testInstance.Assert(t => t.ReferenceEqual(fake, fake.CreateDeepClone()));

        fake.VerifyAllCalls(Times.Never);
    }

    [Theory, RandomData]
    internal void ReferenceNotEqual_NotByValue([Stub] object fake)
    {
        fake.Equals(Arg.Any<object>()).SetupReturn(false, Times.Never);

        _testInstance.ReferenceNotEqual(fake, fake.CreateDeepClone());
        _testInstance.Assert(t => t.ReferenceNotEqual(fake, fake)).Throws<AssertException>();

        fake.VerifyAllCalls(Times.Never);
    }

    [Theory, RandomData]
    internal void ValuesEqual_EqualValid(object value)
    {
        _testInstance.ValuesEqual(value, value.CreateDeepClone());
    }

    [Theory, RandomData]
    internal void ValuesEqual_UnequalInvalid(string value)
    {
        _testInstance.Assert(t => t.ValuesEqual(value, value.CreateVariant())).Throws<AssertException>();
        _testInstance.Assert(t => t.ValuesEqual(null, value)).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal void ValuesEqual_CanHandleNullsNotEqual(object value)
    {
        _testInstance.Assert(t => t.ValuesEqual(value, null)).Throws<AssertException>();
        _testInstance.Assert(t => t.ValuesEqual(null, value)).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal void ValuesNotEqual_UnequalValid(string value)
    {
        _testInstance.ValuesNotEqual(value, value.CreateVariant());
        _testInstance.ValuesNotEqual(null, value);
        _testInstance.ValuesNotEqual(value, null);
    }

    [Theory, RandomData]
    internal void ValuesNotEqual_EqualInvalid(string value)
    {
        _testInstance.Assert(t => t.ValuesNotEqual(value, value.CreateDeepClone())).Throws<AssertException>();
        _testInstance.Assert(t => t.ValuesNotEqual(value, value)).Throws<AssertException>();
        _testInstance.Assert(t => t.ValuesNotEqual(null, null)).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal void AreUnique_UnequalValid(string value)
    {
        _testInstance.AreUnique(value, value.CreateVariant());
        _testInstance.AreUnique(null, value);
        _testInstance.AreUnique(value, null);
    }

    [Theory, RandomData]
    internal void AreUnique_EqualInvalid(string value)
    {
        _testInstance.Assert(t => t.AreUnique(value, value.CreateDeepClone())).Throws<AssertException>();
        _testInstance.Assert(t => t.AreUnique(value, value)).Throws<AssertException>();
        _testInstance.Assert(t => t.AreUnique(null, null)).Throws<AssertException>();
    }
}
