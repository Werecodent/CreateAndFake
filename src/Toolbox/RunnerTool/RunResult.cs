using System.Reflection;

namespace CreateAndFake.RunnerTool;

/// <summary>Holds parameter data for a method.</summary>
/// <param name="method"><inheritdoc cref="Method" path="/summary"/></param>
/// <param name="args"><inheritdoc cref="Args" path="/summary"/></param>
/// <param name="result"><inheritdoc cref="Result" path="/summary"/></param>
public sealed class RunResult(MethodInfo method, IEnumerable<object?> args, object? result)
{
    /// <summary>Associated method.</summary>
    public MethodInfo Method { get; } = method;

    /// <summary>Parameter data for the method.</summary>
    public IEnumerable<object?> Args { get; } = [.. args];

    /// <summary>Return for the method call.</summary>
    public object? Result { get; } = result;
}
