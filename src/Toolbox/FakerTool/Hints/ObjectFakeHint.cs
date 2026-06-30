using CreateAndFake.Design;
using CreateAndFake.FakerTool.Engine;
using CreateAndFake.FakerTool.Proxy;

namespace CreateAndFake.FakerTool.Hints;

/// <summary>Handles faking Span collections for <see cref="IFaker"/>.</summary>
public sealed class ObjectFakeHint : IFakeHint
{
    /// <inheritdoc/>
    public int EnginePriority => (int)FakePriority.ObjectHint;

    /// <inheritdoc/>
    public IEnumerable<Type> SupportedTypes => [typeof(object)];

    /// <inheritdoc/>
    public bool SupportsToFake => true;

    /// <inheritdoc/>
    public bool SupportsToSetup => false;

    /// <inheritdoc/>
    public FakeHintResult TryToFake(Type parent, IEnumerable<Type> interfaces, IFakerChainer faker)
    {
        ArgumentGuard.ThrowIfNull(faker);

        return new(Subclasser.Create(parent, faker.Options, interfaces));
    }

    /// <inheritdoc/>
    public SetupHintResult TryToSetup(IFaked instance, IFakerChainer faker)
    {
        throw new NotSupportedException();
    }
}
