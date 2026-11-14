using Werecodent.CreateAndFake.Design.Tooling;

namespace Werecodent.CreateAndFake.RandomizerTool.Engine;

/// <inheritdoc cref="IRandomizer"/>
public interface IRandomizerEngine : IToolEngine<ICreateHint>
{
    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IRandomizer.Create(Type,RandomizerMod)"/>
    object Create(Type type, IRandomizerChainer chainer);

    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IRandomizer.Inject"/>
    object Inject(Type type, IEnumerable<object?>? values, IRandomizerChainer chainer);
}
