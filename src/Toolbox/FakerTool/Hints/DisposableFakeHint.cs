using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.FakerTool.Engine;
using Werecodent.CreateAndFake.FakerTool.Proxy;
using Werecodent.CreateAndFake.Fluent;

namespace Werecodent.CreateAndFake.FakerTool.Hints;

/// <summary>Handles faking Span collections for <see cref="IFaker"/>.</summary>
public sealed class DisposableFakeHint : IFakeHint
{
    /// <inheritdoc/>
    public int EnginePriority => (int)FakePriority.DisposableHint;

    /// <inheritdoc/>
    public IEnumerable<Type> SupportedTypes => [typeof(IDisposable)];

    /// <inheritdoc/>
    public bool SupportsToFake => false;

    /// <inheritdoc/>
    public bool SupportsToSetup => true;

    /// <inheritdoc/>
    public FakeHintResult TryToFake(Type parent, IEnumerable<Type> interfaces, IFakerChainer faker)
    {
        throw new NotSupportedException();
    }

    /// <inheritdoc/>
    public SetupHintResult TryToSetup(IFaked instance, IFakerChainer faker)
    {
        ArgumentGuard.ThrowIfNull(instance, faker);

        if (instance.GetType().Inherits<IDisposable>())
        {
            new Fake<IDisposable>(instance).Setup(f => f.Dispose(), Behavior.None(Times.Any));
            return new(true);
        }
        else
        {
            return SetupHintResult.None;
        }
    }
}
