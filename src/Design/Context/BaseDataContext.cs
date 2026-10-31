using Werecodent.CreateAndFake.Design.Randomization;

namespace Werecodent.CreateAndFake.Design.Context;

/// <summary>Bundles associated random values from data pools.</summary>
/// <param name="gen"><inheritdoc cref="Gen" path="/summary"/></param>
public abstract class BaseDataContext(IRandom gen)
{
    /// <inheritdoc cref="IRandom" />
    protected IRandom Gen { get; } = gen ?? throw new ArgumentNullException(nameof(gen));
}
