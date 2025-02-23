using System.Reflection;
using CreateAndFake.AsserterTool;
using CreateAndFake.FakerTool;

namespace CreateAndFake.Tests.AsserterTool;

public sealed class AsserterTests
{
    private readonly Asserter _testInstance = new(Tools.Asserter.Options);

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

        fake.Assert().Called(Times.Never);
    }

    [Theory, RandomData]
    internal void ReferenceNotEqual_NotByValue([Stub] object fake)
    {
        fake.Equals(Arg.Any<object>()).SetupReturn(false, Times.Never);

        _testInstance.ReferenceNotEqual(fake, fake.CreateDeepClone());
        _testInstance.Assert(t => t.ReferenceNotEqual(fake, fake)).Throws<AssertException>();

        fake.Assert().Called(Times.Never);
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
