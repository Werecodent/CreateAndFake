using System.Reflection;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Fluent.AssertCalls;

namespace Werecodent.CreateAndFake.Tests.Fluent.TaskAssertUnwrapping;

public sealed class TaskAssertStringExtensionsTests
{
    private static readonly char[] _Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

    private int _modCount;

    private readonly AsserterMod _mod;

    public TaskAssertStringExtensionsTests()
    {
        _modCount = 0;
        _mod = opt =>
        {
            _modCount++;
            return opt;
        };
    }

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
    internal async Task Contains_Forwarded(string data)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        char someChar = Tools.Gen.NextItem(data);
        char uniqueChar = Tools.Gen.NextItem(_Letters.Except(data));

        await Task.FromResult(data.Assert()).Contains(someChar);
        await Task.FromResult(data.Assert()).Contains(someChar, _mod);
        await Task.FromResult(data.Assert()).Contains(someChar.ToString());
        await Task.FromResult(data.Assert()).Contains(someChar.ToString(), _mod);
        await Task.FromResult(data.Assert())
            .Contains(uniqueChar)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .Contains(uniqueChar, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .Contains(uniqueChar.ToString())
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .Contains(uniqueChar.ToString(), _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(4);
    }

    [Theory, RandomData]
    internal async Task ContainsNot_Forwarded(string data)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        char someChar = Tools.Gen.NextItem(data);
        char uniqueChar = Tools.Gen.NextItem(_Letters.Except(data));

        await Task.FromResult(data.Assert()).ContainsNot(uniqueChar);
        await Task.FromResult(data.Assert()).ContainsNot(uniqueChar, _mod);
        await Task.FromResult(data.Assert()).ContainsNot(uniqueChar.ToString());
        await Task.FromResult(data.Assert()).ContainsNot(uniqueChar.ToString(), _mod);
        await Task.FromResult(data.Assert())
            .ContainsNot(someChar)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .ContainsNot(someChar, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .ContainsNot(someChar.ToString())
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .ContainsNot(someChar.ToString(), _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(4);
    }

    [Theory, RandomData]
    internal async Task StartsWith_Forwarded(string data)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        char firstChar = data[0];
        char otherChar = firstChar.Tools().Variant();

        await Task.FromResult(data.Assert()).StartsWith(firstChar);
        await Task.FromResult(data.Assert()).StartsWith(firstChar, _mod);
        await Task.FromResult(data.Assert()).StartsWith(firstChar.ToString());
        await Task.FromResult(data.Assert()).StartsWith(firstChar.ToString(), _mod);
        await Task.FromResult(data.Assert())
            .StartsWith(otherChar)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .StartsWith(otherChar, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .StartsWith(otherChar.ToString())
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .StartsWith(otherChar.ToString(), _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(4);
    }

    [Theory, RandomData]
    internal async Task StartsNotWith_Forwarded(string data)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        char firstChar = data[0];
        char otherChar = firstChar.Tools().Variant();

        await Task.FromResult(data.Assert()).StartsNotWith(otherChar);
        await Task.FromResult(data.Assert()).StartsNotWith(otherChar, _mod);
        await Task.FromResult(data.Assert()).StartsNotWith(otherChar.ToString());
        await Task.FromResult(data.Assert()).StartsNotWith(otherChar.ToString(), _mod);
        await Task.FromResult(data.Assert())
            .StartsNotWith(firstChar)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .StartsNotWith(firstChar, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .StartsNotWith(firstChar.ToString())
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .StartsNotWith(firstChar.ToString(), _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(4);
    }

    [Theory, RandomData]
    internal async Task EndsWith_Forwarded([Size(3)] string data)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        char lastChar = data[2];
        char otherChar = lastChar.Tools().Variant();

        await Task.FromResult(data.Assert()).EndsWith(lastChar);
        await Task.FromResult(data.Assert()).EndsWith(lastChar, _mod);
        await Task.FromResult(data.Assert()).EndsWith(lastChar.ToString());
        await Task.FromResult(data.Assert()).EndsWith(lastChar.ToString(), _mod);
        await Task.FromResult(data.Assert())
            .EndsWith(otherChar)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .EndsWith(otherChar, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .EndsWith(otherChar.ToString())
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .EndsWith(otherChar.ToString(), _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(4);
    }

    [Theory, RandomData]
    internal async Task EndsNotWith_Forwarded([Size(3)] string data)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        char lastChar = data[2];
        char otherChar = lastChar.Tools().Variant();

        await Task.FromResult(data.Assert()).EndsNotWith(otherChar);
        await Task.FromResult(data.Assert()).EndsNotWith(otherChar, _mod);
        await Task.FromResult(data.Assert()).EndsNotWith(otherChar.ToString());
        await Task.FromResult(data.Assert()).EndsNotWith(otherChar.ToString(), _mod);
        await Task.FromResult(data.Assert())
            .EndsNotWith(lastChar)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .EndsNotWith(lastChar, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .EndsNotWith(lastChar.ToString())
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .EndsNotWith(lastChar.ToString(), _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(4);
    }
}
