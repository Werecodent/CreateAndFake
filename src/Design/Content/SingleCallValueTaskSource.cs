using System.Reflection;
using System.Threading.Tasks.Sources;
using CreateAndFake.Design.Comparisons;
using CreateAndFake.Design.Exceptions;

namespace CreateAndFake.Design.Content;

/// <summary>Used to enforce resolving a <see cref="ValueTask"/> only once.</summary>
/// <param name="id"><inheritdoc cref="Id" path="/summary"/></param>
public sealed class SingleCallValueTaskSource(Guid? id = null)
    : IDeepCloneable<SingleCallValueTaskSource>,
        IEquatable<SingleCallValueTaskSource>,
        IValueTaskSource
{
    /// <summary>How to access the source via the containing <see cref="ValueTask"/>.</summary>
    private static readonly FieldInfo _TaskSourceGrabber = typeof(ValueTask).GetField(
        "_obj",
        BindingFlags.Instance | BindingFlags.NonPublic
    )!;

    /// <summary>How to access a <see cref="ValueTask"/>'s token.</summary>
    private static readonly FieldInfo _TaskTokenGrabber = typeof(ValueTask).GetField(
        "_token",
        BindingFlags.Instance | BindingFlags.NonPublic
    )!;

    /// <summary>Identifier representing <see langword="this"/> task.</summary>
    public Guid Id { get; } = id ?? Guid.NewGuid();

    /// <summary>If the result has already been retrieved.</summary>
    private bool _called = false;

    /// <inheritDoc/>
    public void GetResult(short token)
    {
        if (_called)
        {
            throw new ValueTaskRepeatedAccessException(null);
        }
        else
        {
            _called = true;
        }
    }

    /// <inheritDoc/>
    public ValueTaskSourceStatus GetStatus(short token)
    {
        return ValueTaskSourceStatus.Succeeded;
    }

    /// <inheritDoc/>
    public void OnCompleted(
        Action<object?> continuation,
        object? state,
        short token,
        ValueTaskSourceOnCompletedFlags flags
    )
    {
        throw new NotSupportedException();
    }

    /// <inheritdoc/>
    public bool Equals(SingleCallValueTaskSource? other)
    {
        return other is not null && Id == other.Id;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return Equals(obj as SingleCallValueTaskSource);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <inheritdoc/>
    public SingleCallValueTaskSource DeepClone()
    {
        return new SingleCallValueTaskSource(Id);
    }

    /// <summary>Retrieves the source from a <paramref name="task"/>.</summary>
    /// <param name="task">The task potentially containing the source.</param>
    /// <returns>
    ///     The <paramref name="task"/>'s source if it's a <see cref="SingleCallValueTaskSource"/>,
    ///     <see langword="null"/> otherwise.
    /// </returns>
    public static SingleCallValueTaskSource? ExtractFrom(ValueTask task)
    {
        return _TaskSourceGrabber.GetValue(task) as SingleCallValueTaskSource;
    }

    /// <summary>Retrieves the token from a <paramref name="task"/>.</summary>
    /// <param name="task">The task containing the token.</param>
    /// <returns>The <paramref name="task"/>'s opaque token.</returns>
    public static short ExtractTokenFrom(ValueTask task)
    {
        return (short)_TaskTokenGrabber.GetValue(task)!;
    }
}
