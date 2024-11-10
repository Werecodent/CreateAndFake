using System.Text;
using CreateAndFake.Toolbox.MutatorTool;
using CreateAndFake.Toolbox.ValuerTool;

#pragma warning disable CA1307 // Specify StringComparison for clarity: Not available for all versions.

namespace CreateAndFake.Toolbox.AsserterTool.Fluent;

/// <summary>Handles common <see cref="object"/> assertion calls.</summary>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <param name="actual"><inheritdoc cref="Actual" path="/summary"/></param>
public abstract class AssertObjectBase<T>(AsserterOptions options, object? actual) where T : AssertObjectBase<T>
{
    /// <summary>Configured options for <c>this</c>.</summary>
    protected AsserterOptions Options { get; } = options ?? throw new ArgumentNullException(nameof(options));

    /// <summary>Instance to run assertion checks with.</summary>
    protected object? Actual { get; } = actual;

    /// <summary>Verifies <c>actual</c> equals <paramref name="expected"/> by value.</summary>
    /// <param name="expected">Instance to compare against.</param>
    /// <param name="details">Optional failure details to include.</param>
    /// <returns>Chainer to make additional assertions with.</returns>
    /// <exception cref="AssertException">If the comparison fails to match the expected behavior.</exception>
    public AssertChainer<T> Is(object? expected, string? details = null)
    {
        return ValuesEqual(expected, details);
    }

    /// <summary>Verifies <c>actual</c> unequals <paramref name="expected"/> by value.</summary>
    /// <inheritdoc cref="Is"/>
    public AssertChainer<T> IsNot(object? expected, string? details = null)
    {
        return ValuesNotEqual(expected, details);
    }

    /// <summary>Verifies <c>actual</c> equals <paramref name="expected"/> by reference.</summary>
    /// <inheritdoc cref="Is"/>
    public virtual AssertChainer<T> ReferenceEqual(object? expected, string? details = null)
    {
        if (!ReferenceEquals(expected, Actual))
        {
            throw new AssertException("References failed to equal.", details, Options.Gen.InitialSeed);
        }
        return ToChainer();
    }

    /// <summary>Verifies <c>actual</c> unequals <paramref name="expected"/> by reference.</summary>
    /// <inheritdoc cref="Is"/>
    public virtual AssertChainer<T> ReferenceNotEqual(object? expected, string? details = null)
    {
        if (ReferenceEquals(expected, Actual))
        {
            throw new AssertException("References failed to not equal.", details, Options.Gen.InitialSeed);
        }
        return ToChainer();
    }

    /// <inheritdoc cref="Is"/>
    public virtual AssertChainer<T> ValuesEqual(object? expected, string? details = null)
    {
        Difference[] differences = Options.Valuer.Compare(expected, Actual).ToArray();
        if (differences.Length > 0)
        {
            throw new AssertException($"Value equality failed for type '{GetTypeName(expected)}'.",
                details, Options.Gen.InitialSeed, string.Join<Difference>(Environment.NewLine, differences));
        }
        return ToChainer();
    }

    /// <inheritdoc cref="IsNot"/>
    public virtual AssertChainer<T> ValuesNotEqual(object? expected, string? details = null)
    {
        if (!Options.Valuer.Compare(expected, Actual).Any())
        {
            throw new AssertException(
                $"Value inequality failed for type '{GetTypeName(expected)}'.",
                details, Options.Gen.InitialSeed, expected?.ToString());
        }
        return ToChainer();
    }

    /// <summary>Verifies <c>actual</c> shares no data with <paramref name="expected"/>.</summary>
    /// <param name="expected">Instance to compare against.</param>
    /// <param name="details">Optional failure details to include.</param>
    /// <returns>Chainer to make additional assertions with.</returns>
    /// <exception cref="AssertException">If the comparison fails to match the expected behavior.</exception>
    /// <remarks>Ignores types with too small of range for unique randomization.</remarks>
    public virtual AssertChainer<T> UniqueFrom(object? expected, string? details = null)
    {
        _ = ReferenceNotEqual(expected, details);

        int i = 0;
        StringBuilder contents = new();
        foreach (object value in ContentMap.Extract(Actual).FindSharedContent(Options.Valuer, ContentMap.Extract(expected)))
        {
            _ = contents.Append('#').Append(i++).Append(':').Append(value).AppendLine();
        }

        if (i != 0)
        {
            throw new AssertException(
                $"Expected no shared content, but had '{i}' shared items.",
                details, Options.Gen.InitialSeed, contents.ToString());
        }

        return ToChainer();
    }

    /// <summary>Throws an assert exception.</summary>
    /// <param name="details">Optional failure details to include.</param>
    public virtual void Fail(string? details = null)
    {
        throw new AssertException("Test failed.", details, Options.Gen.InitialSeed, Actual?.ToString());
    }

    /// <summary>Stipulates the test is successful if it reaches this point.</summary>
    public virtual void Pass() { }

    /// <summary>Finds a suitable <c>Type</c> name to use for assertion messages.</summary>
    /// <param name="expected">Instance being compared to <c>actual</c>.</param>
    /// <returns>The <c>Type</c> name to use if found; <c>null</c> otherwise.</returns>
    protected string? GetTypeName(object? expected)
    {
        return ExpandTypeName((expected ?? Actual)?.GetType());
    }

    /// <summary>Builds <c>Type</c> name with generic argument names.</summary>
    /// <param name="type"><c>Type</c> to describe.</param>
    /// <returns>The built name.</returns>
    protected static string? ExpandTypeName(Type? type)
    {
        if (type != null && type.IsGenericType)
        {
            return string.Concat(
                type.Name.Substring(0, type.Name.IndexOf('`')),
                "<",
                string.Join(",", type.GetGenericArguments().Select(ExpandTypeName)),
                ">");
        }
        else
        {
            return type?.Name;
        }
    }

    /// <summary>Converts <c>this</c> to a chainer for additional assertions on <c>actual</c>.</summary>
    /// <returns>The created chainer.</returns>
    protected internal AssertChainer<T> ToChainer()
    {
        return new AssertChainer<T>((T)this, Options);
    }
}

#pragma warning restore CA1307 // Specify StringComparison for clarity
