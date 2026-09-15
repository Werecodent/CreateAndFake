using System.Reflection;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Fluent.AssertCalls;

namespace Werecodent.CreateAndFake.Tests.Fluent.TaskAssertUnwrapping;

public static class TaskAssertStringExtensionsTests
{
    private static readonly char[] _Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

    [Fact]
    internal static Task TaskAssertStringExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskAssertStringExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Fact]
    internal static Task TaskAssertStringExtensions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(TaskAssertStringExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Fact]
    internal static void TaskAssertStringExtensions_MatchesEveryMethod()
    {
        typeof(AssertStringBase<>)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .OrderBy(m => m.Name)
            .Select(m => m.Name)
            .Assert()
            .Is(
                typeof(TaskAssertStringExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .OrderBy(m => m.Name)
                    .Select(m => m.Name)
            );
    }

    [Theory, RandomData]
    internal static async Task Contains_Forwarded(string data)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        char someChar = Tools.Gen.NextItem(data);
        char uniqueChar = Tools.Gen.NextItem(_Letters.Except(data));

        await Task.FromResult(data.Assert()).Contains(someChar);
        await Task.FromResult(data.Assert()).Contains(someChar, mod);
        await Task.FromResult(data.Assert()).Contains(someChar.ToString());
        await Task.FromResult(data.Assert()).Contains(someChar.ToString(), mod);
        await Task.FromResult(data.Assert())
            .Contains(uniqueChar)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .Contains(uniqueChar, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .Contains(uniqueChar.ToString())
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .Contains(uniqueChar.ToString(), mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(4);
    }

    [Theory, RandomData]
    internal static async Task ContainsNot_Forwarded(string data)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        char someChar = Tools.Gen.NextItem(data);
        char uniqueChar = Tools.Gen.NextItem(_Letters.Except(data));

        await Task.FromResult(data.Assert()).ContainsNot(uniqueChar);
        await Task.FromResult(data.Assert()).ContainsNot(uniqueChar, mod);
        await Task.FromResult(data.Assert()).ContainsNot(uniqueChar.ToString());
        await Task.FromResult(data.Assert()).ContainsNot(uniqueChar.ToString(), mod);
        await Task.FromResult(data.Assert())
            .ContainsNot(someChar)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .ContainsNot(someChar, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .ContainsNot(someChar.ToString())
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .ContainsNot(someChar.ToString(), mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(4);
    }

    [Theory, RandomData]
    internal static async Task StartsWith_Forwarded(string data)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        char firstChar = data[0];
        char otherChar = firstChar.Tools().Variant();

        await Task.FromResult(data.Assert()).StartsWith(firstChar);
        await Task.FromResult(data.Assert()).StartsWith(firstChar, mod);
        await Task.FromResult(data.Assert()).StartsWith(firstChar.ToString());
        await Task.FromResult(data.Assert()).StartsWith(firstChar.ToString(), mod);
        await Task.FromResult(data.Assert())
            .StartsWith(otherChar)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .StartsWith(otherChar, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .StartsWith(otherChar.ToString())
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .StartsWith(otherChar.ToString(), mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(4);
    }

    [Theory, RandomData]
    internal static async Task StartsNotWith_Forwarded(string data)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        char firstChar = data[0];
        char otherChar = firstChar.Tools().Variant();

        await Task.FromResult(data.Assert()).StartsNotWith(otherChar);
        await Task.FromResult(data.Assert()).StartsNotWith(otherChar, mod);
        await Task.FromResult(data.Assert()).StartsNotWith(otherChar.ToString());
        await Task.FromResult(data.Assert()).StartsNotWith(otherChar.ToString(), mod);
        await Task.FromResult(data.Assert())
            .StartsNotWith(firstChar)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .StartsNotWith(firstChar, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .StartsNotWith(firstChar.ToString())
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .StartsNotWith(firstChar.ToString(), mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(4);
    }

    [Theory, RandomData]
    internal static async Task EndsWith_Forwarded([Size(3)] string data)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        char lastChar = data[2];
        char otherChar = lastChar.Tools().Variant();

        await Task.FromResult(data.Assert()).EndsWith(lastChar);
        await Task.FromResult(data.Assert()).EndsWith(lastChar, mod);
        await Task.FromResult(data.Assert()).EndsWith(lastChar.ToString());
        await Task.FromResult(data.Assert()).EndsWith(lastChar.ToString(), mod);
        await Task.FromResult(data.Assert())
            .EndsWith(otherChar)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .EndsWith(otherChar, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .EndsWith(otherChar.ToString())
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .EndsWith(otherChar.ToString(), mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(4);
    }

    [Theory, RandomData]
    internal static async Task EndsNotWith_Forwarded([Size(3)] string data)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        char lastChar = data[2];
        char otherChar = lastChar.Tools().Variant();

        await Task.FromResult(data.Assert()).EndsNotWith(otherChar);
        await Task.FromResult(data.Assert()).EndsNotWith(otherChar, mod);
        await Task.FromResult(data.Assert()).EndsNotWith(otherChar.ToString());
        await Task.FromResult(data.Assert()).EndsNotWith(otherChar.ToString(), mod);
        await Task.FromResult(data.Assert())
            .EndsNotWith(lastChar)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .EndsNotWith(lastChar, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .EndsNotWith(lastChar.ToString())
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .EndsNotWith(lastChar.ToString(), mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(4);
    }
}
