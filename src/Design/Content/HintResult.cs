namespace CreateAndFake.Design.Content;

/// <summary>Possible result from a tool hint.</summary>
/// <param name="hasData"><inheritdoc cref="HasData" path="/summary"/></param>
/// <param name="data"><inheritdoc cref="Data" path="/summary"/></param>
public abstract class HintResult<T>(bool hasData, T data)
{
    /// <summary>If the hint was successful and <see cref="Data"/> is populated.</summary>
    public bool HasData { get; } = hasData;

    /// <summary>Result of the hint if <see cref="HasData"/> is <c>true</c>.</summary>
    public T Data { get; } = data;
}