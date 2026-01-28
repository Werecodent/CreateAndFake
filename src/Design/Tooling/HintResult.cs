using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Tooling;

/// <summary>Execution result of a tool hint with potential data if successful.</summary>
/// <typeparam name="T">Result <see cref="Type"/> for the hint.</typeparam>
/// <param name="hasData"><inheritdoc cref="HasData" path="/summary"/></param>
/// <param name="data"><inheritdoc cref="Data" path="/summary"/></param>
public abstract class HintResult<T>(bool hasData, T data)
{
    /// <summary>If the hint was successful and <see cref="Data"/> is populated.</summary>
    public bool HasData { get; } = hasData;

    /// <summary>Result of the hint if <see cref="HasData"/> is <see langword="true"/>.</summary>
    public T Data { get; } = data;

    /// <inheritdoc/>
    public override string ToString()
    {
        return TypeDescriber.ExpandedName(GetType());
    }
}
