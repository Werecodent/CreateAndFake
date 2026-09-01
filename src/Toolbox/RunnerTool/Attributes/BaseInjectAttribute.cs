using System.Collections.Specialized;
using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.FakerTool.Proxy;

namespace Werecodent.CreateAndFake.RunnerTool.Attributes;

/// <summary>Flag to create the attached parameter using value injection of previous parameters.</summary>.
/// <seealso cref="IRunner.CreateFor(MethodBase,CancellationToken,RunnerMod)"/>
/// <seealso cref="RandomizerTool.IRandomizer.Inject"/>
public abstract class BaseInjectAttribute : ParameterHintAttribute
{
    /// <inheritdoc/>
    protected internal override object? CreateParameterValue(
        ParameterInfo param,
        MethodBase method,
        OrderedDictionary args,
        RunnerOptions localOptions
    )
    {
        ArgumentGuard.ThrowIfNull(param, args, localOptions);

        return localOptions.Randomizer.Inject(
            param.ParameterType,
            [.. args.Values.Cast<object>().Where(a => a is Fake or IFaked).Reverse()]
        );
    }
}
