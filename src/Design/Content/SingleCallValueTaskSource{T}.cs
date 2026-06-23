using System.Reflection;
using System.Threading.Tasks.Sources;
using CreateAndFake.Design.Exceptions;

namespace CreateAndFake.Design.Content;

/// <summary>Used to enforce resolving a <see cref="ValueTask{T}"/> only once.</summary>
/// <typeparam name="T">The contained result <see cref="Type"/>.</typeparam>
/// <param name="rawResult"><inheritdoc cref="RawResult" path="/summary"/></param>
public sealed class SingleCallValueTaskSource<T>(T rawResult) : IValueTaskSource<T>
{
    /// <summary>How to access the source via the containing <see cref="ValueTask{T}"/>.</summary>
    private static readonly FieldInfo _TaskSourceGrabber = typeof(ValueTask<T>).GetField(
        "_obj",
        BindingFlags.Instance | BindingFlags.NonPublic
    );

    /// <summary>How to access a <see cref="ValueTask{T}"/>'s token.</summary>
    private static readonly FieldInfo _TaskTokenGrabber = typeof(ValueTask<T>).GetField(
        "_token",
        BindingFlags.Instance | BindingFlags.NonPublic
    );

    /// <summary>Stored result to return.</summary>
    public T RawResult { get; } = rawResult;

    /// <summary>If the result has already been retrieved.</summary>
    private bool _called = false;

    /// <inheritDoc/>
    public T GetResult(short token)
    {
        if (_called)
        {
            throw new ValueTaskRepeatedAccessException(null);
        }
        else
        {
            _called = true;
            return RawResult;
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

    /// <summary>Retrieves the source from a <paramref name="task"/>.</summary>
    /// <param name="task">The task potentially containing the source.</param>
    /// <returns>
    ///     The <paramref name="task"/>'s source if it's a <see cref="SingleCallValueTaskSource{T}"/>,
    ///     <see langword="null"/> otherwise.
    /// </returns>
    public static SingleCallValueTaskSource<T>? ExtractFrom(ValueTask<T> task)
    {
        return _TaskSourceGrabber.GetValue(task) as SingleCallValueTaskSource<T>;
    }

    /// <summary>Retrieves the token from a <paramref name="task"/>.</summary>
    /// <param name="task">The task containing the token.</param>
    /// <returns>The <paramref name="task"/>'s opaque token.</returns>
    public static short ExtractTokenFrom(ValueTask<T> task)
    {
        return (short)_TaskTokenGrabber.GetValue(task)!;
    }
}
