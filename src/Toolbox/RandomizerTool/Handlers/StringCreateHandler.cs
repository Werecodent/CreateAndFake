using Werecodent.CreateAndFake.RandomizerTool.Engine;

namespace Werecodent.CreateAndFake.RandomizerTool.Handlers;

internal sealed class StringCreateHandler : ICreateHandler
{
    public Type? SupportedType => typeof(string);

    public object? CreateSupported(IRandomizerChainer randomizer)
    {
        char[] data = new char[randomizer.Options.NextStringSize()];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = randomizer.Options.Gen.NextItem(randomizer.Options.StringCharacterSet);
        }
        return new string(data);
    }
}
