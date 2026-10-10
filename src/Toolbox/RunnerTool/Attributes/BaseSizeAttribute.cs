using System.Collections.Specialized;
using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Content;

namespace Werecodent.CreateAndFake.RunnerTool.Attributes;

/// <summary>
///     Flag to create the attached collection with between <paramref name="min"/> and <paramref name="max"/> items.
/// </summary>
/// <param name="min"><inheritdoc cref="Min" path="/summary"/></param>
/// <param name="max"><inheritdoc cref="Max" path="/summary"/></param>
/// <seealso cref="IRunner.CreateFor(MethodBase,CancellationToken,RunnerMod)"/>
[ExcludeFromCreateAndFake]
public abstract class BaseSizeAttribute(int min, int max) : ParameterHintAttribute
{
    /// <summary>Inclusive lower boundary for the generated collection size.</summary>
    public int Min { get; } = min;

    /// <summary>Inclusive upper boundary for the generated collection size.</summary>
    public int Max { get; } = max;

    /// <summary>Flag to create the attached collection with <paramref name="count"/> items.</summary>
    /// <param name="count">Number of items to generate and populate the attached collection with.</param>
    /// <seealso cref="IRunner.CreateFor(MethodBase,CancellationToken,RunnerMod)"/>
    protected BaseSizeAttribute(int count)
        : this(count, count) { }

    /// <inheritdoc/>
    protected internal override object? CreateParameterValue(
        ParameterInfo param,
        MethodBase method,
        OrderedDictionary args,
        RunnerOptions localOptions
    )
    {
        ArgumentGuard.ThrowIfNull(param, localOptions);

        return localOptions.Randomizer.Create(
            param.ParameterType,
            opt =>
                opt with
                {
                    CollectionMinSize = Min,
                    CollectionMaxSize = Max,
                    StringMinSize = Min,
                    StringMaxSize = Max,
                    NestedOptions = opt,
                }
        );
    }
}
