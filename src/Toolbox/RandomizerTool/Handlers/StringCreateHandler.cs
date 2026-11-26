using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.RandomizerTool.Engine;

namespace Werecodent.CreateAndFake.RandomizerTool.Handlers;

/// <summary>Handles creation of strings.</summary>
internal sealed class StringCreateHandler : ICreateHandler
{
    /// <inheritdoc/>
    public Type? SupportedType => typeof(string);

    /// <inheritdoc/>
    public object? CreateSupported(IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

        char[] data = new char[randomizer.Options.NextStringSize()];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = randomizer.Options.Gen.NextItem(randomizer.Options.StringCharacterSet);
        }
        return new string(data);
    }
}
