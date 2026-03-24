using CreateAndFake.Design.Types;

namespace CreateAndFake.Design.Tooling;

/// <inheritdoc cref="IHintResult{T}"/>
/// <param name="hasData"><inheritdoc cref="HasData" path="/summary"/></param>
/// <param name="data"><inheritdoc cref="Data" path="/summary"/></param>
public abstract class HintResult<T>(bool hasData, T data) : IHintResult<T>
{
    /// <inheritdoc/>
    public bool HasData { get; } = hasData;

    /// <inheritdoc/>
    public T Data { get; } = data;

    /// <inheritdoc/>
    public override string ToString()
    {
        return GenericTypeConverter.ExpandedName(GetType());
    }
}
