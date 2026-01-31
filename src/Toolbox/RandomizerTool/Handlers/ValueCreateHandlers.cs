using CreateAndFake.Design.Randomization;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Handlers;

internal static class ValueCreateHandlers
{
    internal static IEnumerable<ICreateHandler> Handlers =>
        ValueRandom.SupportedTypes.SelectMany<Type, ICreateHandler>(t =>
            [
                new FactoryCreateHandler(t, rand => rand.Options.Gen.Next(t)),
                new FactoryCreateHandler(
                    typeof(Nullable<>).MakeGenericType(t),
                    rand => rand.Options.Gen.Next(t)
                ),
            ]
        );
}
