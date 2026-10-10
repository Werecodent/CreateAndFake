using Microsoft.Extensions.Configuration;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.DuplicatorTool;
using Werecodent.CreateAndFake.ExtractorTool;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.MutatorTool;
using Werecodent.CreateAndFake.RandomizerTool.Engine;
using Werecodent.CreateAndFake.RunnerTool;
using Werecodent.CreateAndFake.TesterTool;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.RandomizerTool.Handlers;

internal sealed class ConfigurationSectionCreateHandler : ICreateHandler
{
    public Type? SupportedType => typeof(IConfigurationSection);

    public object? CreateSupported(IRandomizerChainer randomizer)
    {
        Fake<IConfigurationSection> config = randomizer.Create<Fake<IConfigurationSection>>();
        Behavior<IConfigurationSection> nullSection = Behavior.Returns<IConfigurationSection>(null);

        config.Setup(c => c.GetSection(nameof(Asserter)), nullSection);
        config.Setup(c => c.GetSection(nameof(Duplicator)), nullSection);
        config.Setup(c => c.GetSection(nameof(Extractor)), nullSection);
        config.Setup(c => c.GetSection(nameof(Faker)), nullSection);
        config.Setup(c => c.GetSection(nameof(Mutator)), nullSection);
        config.Setup(c => c.GetSection(nameof(Randomizer)), nullSection);
        config.Setup(c => c.GetSection(nameof(Runner)), nullSection);
        config.Setup(c => c.GetSection(nameof(Tester)), nullSection);
        config.Setup(c => c.GetSection(nameof(Valuer)), nullSection);

        return config.Dummy;
    }
}
