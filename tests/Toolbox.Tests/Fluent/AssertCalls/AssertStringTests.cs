using System.Reflection;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;
using Werecodent.CreateAndFake.RunnerTool;

namespace Werecodent.CreateAndFake.Tests.Fluent.AssertCalls;

public sealed class AssertStringTests
{
    private static readonly char[] _Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

    private int _modCount;

    private readonly AsserterMod _mod;

    public AssertStringTests()
    {
        _modCount = 0;
        _mod = opt =>
        {
            _modCount++;
            return opt;
        };
    }

    private static readonly TesterMod _Config = opt =>
        opt with
        {
            IgnorableExceptions =
            [
                typeof(AssertException),
                typeof(ToolException),
                typeof(InvalidCastException),
                typeof(UnsupportedException),
                typeof(TargetException),
            ],
        };

    [Fact]
    internal static Task AssertString_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertString>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task AssertString_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertString>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Theory, RandomData]
    internal static void AssertString_SupportSizeTests([Size(5)] string original)
    {
        original
            .Assert()
            .HasCount(5)
            .And()
            .HasCountLessOrExactly(5)
            .And()
            .HasCountLessOrExactly(6)
            .And()
            .HasCountLessThan(6)
            .And()
            .HasCountMoreOrExactly(5)
            .And()
            .HasCountMoreOrExactly(4)
            .And()
            .HasCountMoreThan(4);
    }

    [Theory, RandomData]
    internal static async Task AssertString_CallsAndChains(Injected<AssertString> instance)
    {
        RunResults results = await Tools.Runner.CallMethodsOnAsync(
            instance.Dummy,
            TestContext.Current.CancellationToken
        );
        results
            .RawResults.Where(r => r.Result != null)
            .Where(r =>
                r.Result
                    is not AssertChainer<AssertString>
                        and Task<AssertChainer<AssertEnumerable>>
            )
            .Assert()
            .IsEmpty();
    }

    [Theory, RandomData]
    internal void Contains_Forwarded(string data)
    {
        char someChar = Tools.Gen.NextItem(data);
        char uniqueChar = Tools.Gen.NextItem(_Letters.Except(data));

        data.Assert().Contains(someChar);
        data.Assert().Contains(someChar, _mod);
        data.Assert().Contains(someChar.ToString());
        data.Assert().Contains(someChar.ToString(), _mod);
        data.Assert(d => d.Assert().Contains(uniqueChar)).Throws<AssertException>();
        data.Assert(d => d.Assert().Contains(uniqueChar, _mod)).Throws<AssertException>();
        data.Assert(d => d.Assert().Contains(uniqueChar.ToString())).Throws<AssertException>();
        data.Assert(d => d.Assert().Contains(uniqueChar.ToString(), _mod))
            .Throws<AssertException>();

        _modCount.Assert().Is(4);
    }

    [Theory, RandomData]
    internal void ContainsNot_Forwarded(string data)
    {
        char someChar = Tools.Gen.NextItem(data);
        char uniqueChar = Tools.Gen.NextItem(_Letters.Except(data));

        data.Assert().ContainsNot(uniqueChar);
        data.Assert().ContainsNot(uniqueChar, _mod);
        data.Assert().ContainsNot(uniqueChar.ToString());
        data.Assert().ContainsNot(uniqueChar.ToString(), _mod);
        data.Assert(d => d.Assert().ContainsNot(someChar)).Throws<AssertException>();
        data.Assert(d => d.Assert().ContainsNot(someChar, _mod)).Throws<AssertException>();
        data.Assert(d => d.Assert().ContainsNot(someChar.ToString())).Throws<AssertException>();
        data.Assert(d => d.Assert().ContainsNot(someChar.ToString(), _mod))
            .Throws<AssertException>();

        _modCount.Assert().Is(4);
    }

    [Theory, RandomData]
    internal void StartsWith_Forwarded(string data)
    {
        char firstChar = data[0];
        char otherChar = firstChar.Tools().Variant();

        data.Assert().StartsWith(firstChar);
        data.Assert().StartsWith(firstChar, _mod);
        data.Assert().StartsWith(firstChar.ToString());
        data.Assert().StartsWith(firstChar.ToString(), _mod);
        data.Assert(d => d.Assert().StartsWith(otherChar)).Throws<AssertException>();
        data.Assert(d => d.Assert().StartsWith(otherChar, _mod)).Throws<AssertException>();
        data.Assert(d => d.Assert().StartsWith(otherChar.ToString())).Throws<AssertException>();
        data.Assert(d => d.Assert().StartsWith(otherChar.ToString(), _mod))
            .Throws<AssertException>();

        _modCount.Assert().Is(4);
    }

    [Theory, RandomData]
    internal void StartsNotWith_Forwarded(string data)
    {
        char firstChar = data[0];
        char otherChar = firstChar.Tools().Variant();

        data.Assert().StartsNotWith(otherChar);
        data.Assert().StartsNotWith(otherChar, _mod);
        data.Assert().StartsNotWith(otherChar.ToString());
        data.Assert().StartsNotWith(otherChar.ToString(), _mod);
        data.Assert(d => d.Assert().StartsNotWith(firstChar)).Throws<AssertException>();
        data.Assert(d => d.Assert().StartsNotWith(firstChar, _mod)).Throws<AssertException>();
        data.Assert(d => d.Assert().StartsNotWith(firstChar.ToString())).Throws<AssertException>();
        data.Assert(d => d.Assert().StartsNotWith(firstChar.ToString(), _mod))
            .Throws<AssertException>();

        _modCount.Assert().Is(4);
    }

    [Theory, RandomData]
    internal void EndsWith_Forwarded([Size(3)] string data)
    {
        char lastChar = data[2];
        char otherChar = lastChar.Tools().Variant();

        data.Assert().EndsWith(lastChar);
        data.Assert().EndsWith(lastChar, _mod);
        data.Assert().EndsWith(lastChar.ToString());
        data.Assert().EndsWith(lastChar.ToString(), _mod);
        data.Assert(d => d.Assert().EndsWith(otherChar)).Throws<AssertException>();
        data.Assert(d => d.Assert().EndsWith(otherChar, _mod)).Throws<AssertException>();
        data.Assert(d => d.Assert().EndsWith(otherChar.ToString())).Throws<AssertException>();
        data.Assert(d => d.Assert().EndsWith(otherChar.ToString(), _mod)).Throws<AssertException>();

        _modCount.Assert().Is(4);
    }

    [Theory, RandomData]
    internal void EndsNotWith_Forwarded([Size(3)] string data)
    {
        char lastChar = data[2];
        char otherChar = lastChar.Tools().Variant();

        data.Assert().EndsNotWith(otherChar);
        data.Assert().EndsNotWith(otherChar, _mod);
        data.Assert().EndsNotWith(otherChar.ToString());
        data.Assert().EndsNotWith(otherChar.ToString(), _mod);
        data.Assert(d => d.Assert().EndsNotWith(lastChar)).Throws<AssertException>();
        data.Assert(d => d.Assert().EndsNotWith(lastChar, _mod)).Throws<AssertException>();
        data.Assert(d => d.Assert().EndsNotWith(lastChar.ToString())).Throws<AssertException>();
        data.Assert(d => d.Assert().EndsNotWith(lastChar.ToString(), _mod))
            .Throws<AssertException>();

        _modCount.Assert().Is(4);
    }
}
