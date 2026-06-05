namespace CreateAndFake.Fluent.Chaining;

/// <summary>Chainer enabling additional assertion calls for resulting data.</summary>
/// <typeparam name="T">Assertion base <see cref="Type"/> to chain.</typeparam>
/// <param name="result">Assertion base instance to chain.</param>
public sealed class ResultChainer<T>(T result)
{
    /// <summary>Result returned.</summary>
    public T Result => result;
}
