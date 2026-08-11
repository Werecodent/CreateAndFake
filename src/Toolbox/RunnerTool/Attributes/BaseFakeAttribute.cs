using System.Collections.Specialized;
using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.FakerTool;

namespace Werecodent.CreateAndFake.RunnerTool.Attributes;

/// <summary>Flag to create the attached <see langword="object"/> as a stub with injected random behavior.</summary>
/// <seealso cref="IRunner.CreateFor(MethodBase, CancellationToken)"/>
/// <seealso cref="RandomizerTool.IRandomizer.Inject"/>
/// <seealso cref="IFaker.Stub(Type,IEnumerable{Type})"/>
public abstract class BaseFakeAttribute : ParameterHintAttribute
{
    /// <inheritdoc/>
    protected internal override object CreateParameterValue(
        ParameterInfo param,
        MethodBase method,
        OrderedDictionary args,
        RunnerOptions localOptions
    )
    {
        ArgumentGuard.ThrowIfNull(param, localOptions);

        return (
            (Fake)
                localOptions.Randomizer.Create(typeof(Fake<>).MakeGenericType(param.ParameterType))!
        ).Dummy;
    }
}
