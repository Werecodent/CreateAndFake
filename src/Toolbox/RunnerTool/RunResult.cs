using System.Reflection;

namespace CreateAndFake.RunnerTool;

/// <summary>Call and result for a <paramref name="method"/> call.</summary>
/// <param name="method"><inheritdoc cref="Method" path="/summary"/></param>
/// <param name="args"><inheritdoc cref="Args" path="/summary"/></param>
/// <param name="result"><inheritdoc cref="Result" path="/summary"/></param>
/// <param name="threwException"><inheritdoc cref="ThrewException" path="/summary"/></param>
public sealed class RunResult(
    MethodBase method,
    IEnumerable<object?> args,
    object? result,
    bool threwException
)
{
    /// <summary>Associated method that was ran to produce the <see cref="Result"/>.</summary>
    public MethodBase Method { get; } = method ?? throw new ArgumentNullException(nameof(method));

    /// <summary>Parameter data used to call the <see cref="Method"/>.</summary>
    public IEnumerable<object?> Args { get; } = [.. args];

    /// <summary>Return data for the <see cref="Method"/> call.</summary>
    public object? Result { get; } = result;

    /// <summary>
    ///     If the <see cref="Method"/> call completed successfully and <see cref="Result"/> contains the returned data.
    /// </summary>
    public bool HasSuccessfulResult { get; } =
        !threwException && result?.GetType() != typeof(VoidReturn);

    /// <summary>
    ///     If the <see cref="Method"/> call threw an exception and <see cref="Result"/> contains the exception.
    /// </summary>
    public bool ThrewException { get; } = threwException;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Method.Name}({(HasSuccessfulResult ? "Success" : "Exception")}): {Result?.ToString()}";
    }
}
