namespace Werecodent.CreateAndFake.Design.Tooling;

/// <summary>Execution result of an <see cref="IToolHint"/> with potential resulting data.</summary>
/// <typeparam name="T">Result <see cref="Type"/> for the hint.</typeparam>
public interface IHintResult<out T>
{
    /// <summary>If the <see cref="IToolHint"/> was successful and <see cref="Data"/> is populated.</summary>
    bool HasData { get; }

    /// <summary>Result of the <see cref="IToolHint"/> if <see cref="HasData"/><c>== true</c>.</summary>
    T Data { get; }
}
