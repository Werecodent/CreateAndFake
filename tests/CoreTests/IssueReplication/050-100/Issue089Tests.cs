using CreateAndFake.Toolbox.AsserterTool;

namespace CreateAndFakeTests.IssueReplication;

public static class Issue089Tests
{
    [Theory, RandomData]
    internal static void Issue089_StringAssertions(string text)
    {
        text.Assert().Is(Tools.Duplicator.Copy(text));
        text.Assert().HasCount(text.Length);

        string startSample = text.Substring(0, 2);
        string endSample = text.Substring(text.Length - 2, 2);

        text.Assert().Contains(text.Substring(2, 2));
        text.Assert().ContainsNot(Tools.Mutator.Variant(text));
        text.Assert().StartsWith(startSample);
        text.Assert().StartsNotWith(Tools.Mutator.Variant(startSample));
        text.Assert().EndsWith(endSample);
        text.Assert().EndsNotWith(Tools.Mutator.Variant(endSample));
    }

    [Theory, RandomData]
    internal static void Issue089_InvalidStringAssertions(string text)
    {
        text.Assert(t => t.Assert().IsNot(Tools.Duplicator.Copy(text))).Throws<AssertException>();
        text.Assert(t => t.Assert().HasCount(text.Length + 1)).Throws<AssertException>();

        string startSample = text.Substring(0, 2);
        string endSample = text.Substring(text.Length - 2, 2);

        text.Assert(t => t.Assert().Contains(Tools.Mutator.Variant(text))).Throws<AssertException>();
        text.Assert(t => t.Assert().ContainsNot(text.Substring(2, 2))).Throws<AssertException>();
        text.Assert(t => t.Assert().StartsWith(Tools.Mutator.Variant(startSample))).Throws<AssertException>();
        text.Assert(t => t.Assert().StartsNotWith(startSample)).Throws<AssertException>();
        text.Assert(t => t.Assert().EndsWith(Tools.Mutator.Variant(endSample))).Throws<AssertException>();
        text.Assert(t => t.Assert().EndsNotWith(endSample)).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void Issue089_DelegateAssertions(InvalidOperationException error, object item)
    {
        Action action = () => throw error;
        action.Assert().Throws<InvalidOperationException>().Assert().Is(error);

        Func<object> func = () => throw error;
        func.Assert().Throws<InvalidOperationException>().Assert().Is(error);

        item.Assert(o => throw error).Throws<InvalidOperationException>().Assert().Is(error);

        item.Assert(o => o.Assert().Fail()).Throws<AssertException>().Assert().IsNot(error);
    }

    [Theory, RandomData]
    internal static void Issue089_ComparableAssertions(int value)
    {
        value.Assert().GreaterThan(value - 1);
        value.Assert().GreaterThanOrEqualTo(value - 1);
        value.Assert().GreaterThanOrEqualTo(value);
        value.Assert().LessThanOrEqualTo(value);
        value.Assert().LessThanOrEqualTo(value + 1);
        value.Assert().LessThan(value + 1);

        value.Assert().InRange(value - 1, value + 1);
        value.Assert().InRange(value, value);

        value.Assert().GreaterThanOrIs(value - 1);
        value.Assert().GreaterThanOrIs(value);
        value.Assert().LessThanOrIs(value);
        value.Assert().LessThanOrIs(value + 1);

        ((int?)null).Assert().GreaterThanOrIs(null);
        ((int?)null).Assert().LessThanOrIs(null);
    }

    [Theory, RandomData]
    internal static void Issue089_InvalidComparableAssertions(int value)
    {
        value.Assert(v => v.Assert().GreaterThan(value + 1)).Throws<AssertException>();
        value.Assert(v => v.Assert().GreaterThanOrEqualTo(value + 1)).Throws<AssertException>();
        value.Assert(v => v.Assert().LessThanOrEqualTo(value - 1)).Throws<AssertException>();
        value.Assert(v => v.Assert().LessThan(value - 1)).Throws<AssertException>();

        value.Assert(v => v.Assert().InRange(value + 1, value)).Throws<AssertException>();
        value.Assert(v => v.Assert().InRange(value, value - 1)).Throws<AssertException>();

        value.Assert(v => v.Assert().GreaterThan(null)).Throws<AssertException>();
        value.Assert(v => v.Assert().InRange(null, value)).Throws<AssertException>();
        value.Assert(v => v.Assert().InRange(value, null)).Throws<AssertException>();
        ((int?)null).Assert(v => v.Assert().InRange(value, value)).Throws<AssertException>();
        ((int?)null).Assert(v => v.Assert().GreaterThan(value)).Throws<AssertException>();
    }
}