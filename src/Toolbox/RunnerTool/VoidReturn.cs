namespace CreateAndFake.RunnerTool;

/// <summary>Represents void result for runs.</summary>
public sealed class VoidReturn
{
    /// <summary>Singleton.</summary>
    public static VoidReturn Instance { get; } = new VoidReturn();

    /// <inheritdoc cref="VoidReturn"/>
    private VoidReturn() { }
}
