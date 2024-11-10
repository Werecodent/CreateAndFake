using System.Collections;
using System.Text;

namespace CreateAndFake.Toolbox.AsserterTool.Fluent;

/// <summary>Handles common collection assertion calls.</summary>
/// <param name="collection"><inheritdoc cref="Collection" path="/summary"/></param>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertGroupBase<T>(AsserterOptions options, IEnumerable? collection)
    : AssertObjectBase<T>(options, collection) where T : AssertGroupBase<T>
{
    /// <summary>Collection to run assertion checks with.</summary>
    protected IEnumerable? Collection { get; } = collection;

    /// <summary>Verifies <c>collection</c> is empty.</summary>
    /// <param name="details">Optional failure details to include.</param>
    /// <returns>Chainer to make additional assertions with.</returns>
    /// <exception cref="AssertException">If <c>collection</c> fails to match the expected behavior.</exception>
    public virtual AssertChainer<T> IsEmpty(string? details = null)
    {
        return HasCount(0, details);
    }

    /// <summary>Verifies <c>collection</c> is not empty.</summary>
    /// <inheritdoc cref="IsEmpty"/>
    public virtual AssertChainer<T> IsNotEmpty(string? details = null)
    {
        if (Collection == null)
        {
            throw new AssertException(
                $"Expected collection with elements, but was 'null'.", details, Options.Gen.InitialSeed);
        }
        else if (!Collection.GetEnumerator().MoveNext())
        {
            throw new AssertException(
                "Expected collection with elements, but was empty.", details, Options.Gen.InitialSeed);
        }
        else
        {
            return ToChainer();
        }
    }

    /// <summary>Verifies <c>collection</c> has <paramref name="count"/> elements.</summary>
    /// <param name="count">Size that the <c>collection</c> should be.</param>
    /// <inheritdoc cref="IsEmpty"/>
    public virtual AssertChainer<T> HasCount(int count, string? details = null)
    {
        if (Collection == null)
        {
            throw new AssertException(
                $"Expected collection of '{count}' elements, but was 'null'.", details, Options.Gen.InitialSeed);
        }

        int i = 0;
        StringBuilder contents = new();
        for (IEnumerator data = Collection.GetEnumerator(); data.MoveNext(); i++)
        {
            _ = contents.Append('[').Append(i).Append("]:").Append(data.Current).AppendLine();
        }

        if (i != count)
        {
            throw new AssertException(
                $"Expected collection of '{count}' elements, but was '{i}'.",
                details, Options.Gen.InitialSeed, contents.ToString());
        }

        return ToChainer();
    }

    /// <summary>Verifies <c>collection</c> contains an element equal to <paramref name="content"/> by value.</summary>
    /// <param name="content">Potential element to check for.</param>
    /// <inheritdoc cref="IsEmpty"/>
    public virtual AssertChainer<T> Contains(object? content, string? details = null)
    {
        if (Collection == null)
        {
            throw new AssertException(
                $"Expected collection to contain '{content}', but was 'null'.", details, Options.Gen.InitialSeed);
        }

        int i = 0;
        bool success = false;
        StringBuilder contents = new();
        for (IEnumerator data = Collection.GetEnumerator(); data.MoveNext(); i++)
        {
            success = success || Options.Valuer.Equals(content, data.Current);

            _ = contents.Append('[').Append(i).Append("]:").Append(data.Current).AppendLine();
        }

        if (!success)
        {
            throw new AssertException(
                $"Expected collection to contain '{content}' but didn't.",
                details, Options.Gen.InitialSeed, contents.ToString());
        }

        return ToChainer();
    }

    /// <summary>
    ///     Verifies <c>collection</c> contains an element unequal to <paramref name="content"/> by value.
    /// </summary>
    /// <param name="content">Potential element to check for.</param>
    /// <inheritdoc cref="Contains"/>
    public virtual AssertChainer<T> ContainsNot(object? content, string? details = null)
    {
        if (Collection == null)
        {
            throw new AssertException(
                $"Expected collection to contain '{content}', but was 'null'.", details, Options.Gen.InitialSeed);
        }

        int i = 0;
        bool success = true;
        StringBuilder contents = new();
        for (IEnumerator data = Collection.GetEnumerator(); data.MoveNext(); i++)
        {
            success &= !Options.Valuer.Equals(content, data.Current);

            _ = contents.Append('[').Append(i).Append("]:").Append(data.Current).AppendLine();
        }

        if (!success)
        {
            throw new AssertException(
                $"Expected collection to contain '{content}' but didn't.",
                details, Options.Gen.InitialSeed, contents.ToString());
        }

        return ToChainer();
    }

    /// <inheritdoc/>
    public override void Fail(string? details = null)
    {
        if (Collection == null)
        {
            throw new AssertException($"Test failed.", details, Options.Gen.InitialSeed, (string?)null);
        }

        int i = 0;
        StringBuilder contents = new();
        for (IEnumerator data = Collection.GetEnumerator(); data.MoveNext(); i++)
        {
            _ = contents.Append('[').Append(i).Append("]:").Append(data.Current).AppendLine();
        }

        throw new AssertException("Test failed.", details, Options.Gen.InitialSeed, contents.ToString());
    }
}
