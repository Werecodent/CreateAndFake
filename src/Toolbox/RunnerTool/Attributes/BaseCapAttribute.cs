namespace Werecodent.CreateAndFake.RunnerTool.Attributes;

/// <inheritdoc/>
[CLSCompliant(false)]
public abstract class BaseCapAttribute : BaseCapAttribute<int>
{
    /// <inheritdoc/>
    protected BaseCapAttribute(int min, int max)
        : base(min, max) { }

    /// <inheritdoc/>
    protected BaseCapAttribute(int max)
        : base(max) { }
}
