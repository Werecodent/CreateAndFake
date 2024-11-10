using System.Collections;
using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Toolbox.AsserterTool.Fluent;

namespace CreateAndFake.Toolbox.AsserterTool;

/// <summary>Handles common test scenarios.</summary>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <c>null</c> parameter.</exception>
public class Asserter(AsserterOptions options)
{
    /// <summary>Configured options for <c>this</c>.</summary>
    public AsserterOptions Options { get; } = options ?? throw new ArgumentNullException(nameof(options));

    /// <summary>Runs each case and aggregates exceptions.</summary>
    /// <param name="cases">Assert cases.</param>
    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Rethrows all at end.")]
    public virtual void CheckAll(params Action[] cases)
    {
        if (cases == null)
        {
            return;
        }

        Exception?[] errors = new Exception[cases.Length];
        for (int i = 0; i < errors.Length; i++)
        {
            try
            {
                cases[i].Invoke();
                errors[i] = null;
            }
            catch (Exception e)
            {
                errors[i] = e;
            }
        }

        if (errors.Any(e => e != null))
        {
            throw new AggregateException("Cases failed: " +
                string.Join(", ", Enumerable.Range(0, errors.Length).Where(i => errors[i] != null)) + " -",
                errors.Where(e => e != null).Select(e => e!));
        }
    }

    /// <inheritdoc cref="Fail(Exception,string)"/>
    public virtual void Fail(string? details = null)
    {
        throw new AssertException("Test failed.", details, Options.Gen.InitialSeed);
    }

    /// <summary>Throws an assert exception.</summary>
    /// <param name="exception">Exception that occurred.</param>
    /// <param name="details">Optional failure details to include.</param>
    public virtual void Fail(Exception exception, string? details = null)
    {
        throw new AssertException("Test failed.", details, Options.Gen.InitialSeed, exception);
    }

    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns></returns>
    /// <inheritdoc cref="AssertObjectBase{T}.Is"/>
    public void Is(object? expected, object? actual, string? details = null)
    {
        _ = new AssertObject(Options, actual).Is(expected, details);
    }

    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns></returns>
    /// <inheritdoc cref="AssertObjectBase{T}.IsNot"/>
    public void IsNot(object? expected, object? actual, string? details = null)
    {
        _ = new AssertObject(Options, actual).IsNot(expected, details);
    }

    /// <param name="collection"><inheritdoc cref="AssertGroupBase{T}.Collection" path="/summary"/></param>
    /// <returns></returns>
    /// <inheritdoc cref="AssertGroupBase{T}.IsEmpty"/>
    public virtual void IsEmpty(IEnumerable? collection, string? details = null)
    {
        _ = new AssertGroup(Options, collection).IsEmpty(details);
    }

    /// <param name="collection"><inheritdoc cref="AssertGroupBase{T}.Collection" path="/summary"/></param>
    /// <returns></returns>
    /// <inheritdoc cref="AssertGroupBase{T}.IsNotEmpty"/>
    public virtual void IsNotEmpty(IEnumerable? collection, string? details = null)
    {
        _ = new AssertGroup(Options, collection).IsNotEmpty(details);
    }

    /// <param name="collection"><inheritdoc cref="AssertGroupBase{T}.Collection" path="/summary"/></param>
    /// <returns></returns>
    /// <inheritdoc cref="AssertGroupBase{T}.HasCount"/>
    public virtual void HasCount(int count, IEnumerable? collection, string? details = null)
    {
        _ = new AssertGroup(Options, collection).HasCount(count, details);
    }

    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns></returns>
    /// <inheritdoc cref="AssertObjectBase{T}.ReferenceEqual"/>
    public virtual void ReferenceEqual(object? expected, object? actual, string? details = null)
    {
        _ = new AssertObject(Options, actual).ReferenceEqual(expected, details);
    }

    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns></returns>
    /// <inheritdoc cref="AssertObjectBase{T}.ReferenceNotEqual"/>
    public virtual void ReferenceNotEqual(object? expected, object? actual, string? details = null)
    {
        _ = new AssertObject(Options, actual).ReferenceNotEqual(expected, details);
    }

    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns></returns>
    /// <inheritdoc cref="AssertObjectBase{T}.ValuesEqual"/>
    public virtual void ValuesEqual(object? expected, object? actual, string? details = null)
    {
        _ = new AssertObject(Options, actual).ValuesEqual(expected, details);
    }

    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns></returns>
    /// <inheritdoc cref="AssertObjectBase{T}.ValuesNotEqual"/>
    public virtual void ValuesNotEqual(object? expected, object? actual, string? details = null)
    {
        _ = new AssertObject(Options, actual).ValuesNotEqual(expected, details);
    }

    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns></returns>
    /// <inheritdoc cref="AssertObjectBase{T}.UniqueFrom"/>    
    public virtual void AreUnique(object? expected, object? actual, string? details = null)
    {
        _ = new AssertObject(Options, actual).UniqueFrom(expected, details);
    }

    /// <param name="behavior"><inheritdoc cref="AssertBehaviorBase{T}.Throws" path="/summary"/></param>
    /// <inheritdoc cref="AssertBehaviorBase{T}.Throws"/>
    public virtual T Throws<T>(Action? behavior, string? details = null) where T : Exception
    {
        return new AssertBehavior(Options, behavior).Throws<T>(details);
    }

    /// <param name="behavior"><inheritdoc cref="AssertBehaviorBase{T}.Throws" path="/summary"/></param>
    /// <inheritdoc cref="AssertBehaviorBase{T}.Throws"/>
    public virtual T Throws<T>(Func<object?>? behavior, string? details = null) where T : Exception
    {
        return new AssertBehavior(Options, behavior).Throws<T>(details);
    }
}
