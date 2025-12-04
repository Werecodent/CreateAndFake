using System.Reflection;

namespace CreateAndFake.RunnerTool;

/// <summary>Holds parameter data for a method.</summary>
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
    /// <summary>Associated method.</summary>
    public MethodBase Method { get; } = method;

    /// <summary>Parameter data for the method.</summary>
    public IEnumerable<object?> Args { get; } = [.. args];

    /// <summary>Return for the method call.</summary>
    public object? Result { get; } = result;

    /// <summary>If the method completed and returned data; Result will be the data.</summary>
    public bool HasSuccessfulResult { get; } =
        !threwException && result?.GetType() != typeof(VoidReturn);

    /// <summary>If the method threw an exception; Result will be the exception.</summary>
    public bool ThrewException { get; } = threwException;
}
