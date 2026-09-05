using System.Collections.Specialized;
using System.Reflection;
using Werecodent.CreateAndFake.Design;

namespace Werecodent.CreateAndFake.RunnerTool.Attributes;

/// <summary>Flag to create the attached <see langword="object"/> as a stub with injected random behavior.</summary>
/// <seealso cref="IRunner.CreateFor(MethodBase,CancellationToken,RunnerMod)"/>
/// <seealso cref="RandomizerTool.IRandomizer.Inject"/>
/// <seealso cref="FakerTool.IFaker.Stub(Type,IEnumerable{Type})"/>
public abstract class BaseUniqueAttribute : ParameterHintAttribute
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

        return localOptions.Mutator.UniqueOf(param.ParameterType, args.Values.Cast<object>());
    }
}
