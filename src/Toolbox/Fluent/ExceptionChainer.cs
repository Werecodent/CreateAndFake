namespace CreateAndFake.Fluent;

/// <summary>Chainer enabling additional assertion calls for throws assertions.</summary>
/// <typeparam name="T">Assertion base <see cref="Type"/> to chain.</typeparam>
/// <param name="exception">Assertion base instance to chain.</param>
public sealed class ExceptionChainer<T>(T exception)
    where T : Exception
{
    /// <summary>Includes another assertion on the instance to test.</summary>
    public T That { get; } = exception;

    /// <summary>Includes another assertion on the instance to test.</summary>
    public T Exception => That;
}
