using CreateAndFake.Design;
using CreateAndFake.Toolbox.FakerTool;
using CreateAndFake.Toolbox.FakerTool.Proxy;

namespace CreateAndFake.Toolbox.RandomizerTool.CreateHints;

/// <summary>Handles randomizing <see cref="IFaked"/> instances for <see cref="IRandomizer"/>.</summary>
public sealed class FakedCreateHint : CreateHint<IFaked>
{
    /// <inheritdoc/>
    protected override IFaked Create(RandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer, nameof(randomizer));

        Fake stub = randomizer.Options.Faker.Stub<object>();
        stub.Dummy.FakeMeta.Identifier = randomizer.Create<int>();
        return stub.Dummy;
    }
}
