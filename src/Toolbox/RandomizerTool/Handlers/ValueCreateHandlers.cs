using CreateAndFake.Design.Randomization;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Handlers;

internal static class ValueCreateHandlers
{
    internal static IEnumerable<ICreateHandler> Handlers =>
        ValueRandom.SupportedTypes.Select(t => new FactoryCreateHandler(
            t,
            rand => rand.Options.Gen.Next(t)
        ));
}
