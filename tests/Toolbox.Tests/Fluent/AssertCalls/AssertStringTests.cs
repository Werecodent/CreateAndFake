using System.Reflection;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;
using Werecodent.CreateAndFake.RunnerTool;

namespace Werecodent.CreateAndFake.Tests.Fluent.AssertCalls;

public static class AssertStringTests
{
    private static readonly char[] _Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

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
    internal static void Contains_Forwarded(string data)
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        char someChar = Tools.Gen.NextItem(data);
        char uniqueChar = Tools.Gen.NextItem(_Letters.Except(data));

        data.Assert().Contains(someChar);
        data.Assert().Contains(someChar, mod);
        data.Assert().Contains(someChar.ToString());
        data.Assert().Contains(someChar.ToString(), mod);
        data.Assert(d => d.Assert().Contains(uniqueChar)).Throws<AssertException>();
        data.Assert(d => d.Assert().Contains(uniqueChar, mod)).Throws<AssertException>();
        data.Assert(d => d.Assert().Contains(uniqueChar.ToString())).Throws<AssertException>();
        data.Assert(d => d.Assert().Contains(uniqueChar.ToString(), mod)).Throws<AssertException>();

        modCount.Assert().Is(4);
    }

    [Theory, RandomData]
    internal static void ContainsNot_Forwarded(string data)
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        char someChar = Tools.Gen.NextItem(data);
        char uniqueChar = Tools.Gen.NextItem(_Letters.Except(data));

        data.Assert().ContainsNot(uniqueChar);
        data.Assert().ContainsNot(uniqueChar, mod);
        data.Assert().ContainsNot(uniqueChar.ToString());
        data.Assert().ContainsNot(uniqueChar.ToString(), mod);
        data.Assert(d => d.Assert().ContainsNot(someChar)).Throws<AssertException>();
        data.Assert(d => d.Assert().ContainsNot(someChar, mod)).Throws<AssertException>();
        data.Assert(d => d.Assert().ContainsNot(someChar.ToString())).Throws<AssertException>();
        data.Assert(d => d.Assert().ContainsNot(someChar.ToString(), mod))
            .Throws<AssertException>();

        modCount.Assert().Is(4);
    }

    [Theory, RandomData]
    internal static void StartsWith_Forwarded(string data)
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        char firstChar = data[0];
        char otherChar = firstChar.Tools().Variant();

        data.Assert().StartsWith(firstChar);
        data.Assert().StartsWith(firstChar, mod);
        data.Assert().StartsWith(firstChar.ToString());
        data.Assert().StartsWith(firstChar.ToString(), mod);
        data.Assert(d => d.Assert().StartsWith(otherChar)).Throws<AssertException>();
        data.Assert(d => d.Assert().StartsWith(otherChar, mod)).Throws<AssertException>();
        data.Assert(d => d.Assert().StartsWith(otherChar.ToString())).Throws<AssertException>();
        data.Assert(d => d.Assert().StartsWith(otherChar.ToString(), mod))
            .Throws<AssertException>();

        modCount.Assert().Is(4);
    }

    [Theory, RandomData]
    internal static void StartsNotWith_Forwarded(string data)
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        char firstChar = data[0];
        char otherChar = firstChar.Tools().Variant();

        data.Assert().StartsNotWith(otherChar);
        data.Assert().StartsNotWith(otherChar, mod);
        data.Assert().StartsNotWith(otherChar.ToString());
        data.Assert().StartsNotWith(otherChar.ToString(), mod);
        data.Assert(d => d.Assert().StartsNotWith(firstChar)).Throws<AssertException>();
        data.Assert(d => d.Assert().StartsNotWith(firstChar, mod)).Throws<AssertException>();
        data.Assert(d => d.Assert().StartsNotWith(firstChar.ToString())).Throws<AssertException>();
        data.Assert(d => d.Assert().StartsNotWith(firstChar.ToString(), mod))
            .Throws<AssertException>();

        modCount.Assert().Is(4);
    }

    [Theory, RandomData]
    internal static void EndsWith_Forwarded([Size(3)] string data)
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        char lastChar = data[2];
        char otherChar = lastChar.Tools().Variant();

        data.Assert().EndsWith(lastChar);
        data.Assert().EndsWith(lastChar, mod);
        data.Assert().EndsWith(lastChar.ToString());
        data.Assert().EndsWith(lastChar.ToString(), mod);
        data.Assert(d => d.Assert().EndsWith(otherChar)).Throws<AssertException>();
        data.Assert(d => d.Assert().EndsWith(otherChar, mod)).Throws<AssertException>();
        data.Assert(d => d.Assert().EndsWith(otherChar.ToString())).Throws<AssertException>();
        data.Assert(d => d.Assert().EndsWith(otherChar.ToString(), mod)).Throws<AssertException>();

        modCount.Assert().Is(4);
    }

    [Theory, RandomData]
    internal static void EndsNotWith_Forwarded([Size(3)] string data)
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        char lastChar = data[2];
        char otherChar = lastChar.Tools().Variant();

        data.Assert().EndsNotWith(otherChar);
        data.Assert().EndsNotWith(otherChar, mod);
        data.Assert().EndsNotWith(otherChar.ToString());
        data.Assert().EndsNotWith(otherChar.ToString(), mod);
        data.Assert(d => d.Assert().EndsNotWith(lastChar)).Throws<AssertException>();
        data.Assert(d => d.Assert().EndsNotWith(lastChar, mod)).Throws<AssertException>();
        data.Assert(d => d.Assert().EndsNotWith(lastChar.ToString())).Throws<AssertException>();
        data.Assert(d => d.Assert().EndsNotWith(lastChar.ToString(), mod))
            .Throws<AssertException>();

        modCount.Assert().Is(4);
    }
}
