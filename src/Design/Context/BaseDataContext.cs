using CreateAndFake.Design.Randomization;

namespace CreateAndFake.Design.Context;

/// <summary>Bundles associated random values from data pools.</summary>
public abstract class BaseDataContext
{
    /// <inheritdoc cref="IRandom" />
    protected IRandom Gen { get; }

    /// <inheritdoc cref="BaseDataContext"/>
    /// <param name="gen"><inheritdoc cref="Gen" path="/summary"/></param>
    internal BaseDataContext(IRandom gen)
    {
        Gen = gen ?? throw new ArgumentNullException(nameof(gen));
    }
}