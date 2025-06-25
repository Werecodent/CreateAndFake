using CreateAndFake.Design.Tooling;

namespace CreateAndFake.RandomizerTool.Engine;

/// <inheritdoc cref="IRandomizer"/>
public interface IRandomizerEngine : IToolEngine<CreateHint>
{
    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IRandomizer.Create(Type,RandomizerMod)"/>
    object Create(Type type, IRandomizerChainer chainer);

    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IRandomizer.Inject"/>
    object Inject(Type type, IEnumerable<object?>? values, IRandomizerChainer chainer);
}
