using System.Collections.Specialized;
using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Randomization;

namespace Werecodent.CreateAndFake.RunnerTool.Attributes;

/// <summary>Flag to create the attached value within a range.</summary>
/// <typeparam name="T">Value Type to generate.</typeparam>
[CLSCompliant(false)]
public abstract class BaseCapAttribute<T> : ParameterHintAttribute
    where T : struct, IComparable, IComparable<T>, IEquatable<T>
{
    private readonly bool _minSet;

    /// <summary>Lower boundary for the generated value.</summary>
    public T Min { get; }

    /// <summary>Upper boundary for the generated value.</summary>
    public T Max { get; }

    /// <summary>
    ///     Flag to create the attached value specifically between <paramref name="min"/> and <paramref name="max"/>.
    /// </summary>
    /// <param name="min"><inheritdoc cref="Min" path="/summary"/> (Inclusive)</param>
    /// <param name="max"><inheritdoc cref="Max" path="/summary"/> (Inclusive)</param>
    /// <seealso cref="IRandom.Next{T}(T,T)"/>
    protected BaseCapAttribute(T min, T max)
    {
        _minSet = true;
        Min = min;
        Max = max;
    }

    /// <summary>Flag to create the attached value specifically below <paramref name="max"/>.</summary>
    /// <param name="max"><inheritdoc cref="Max" path="/summary"/> (Exclusive)</param>
    /// <seealso cref="IRandom.Next{T}(T)"/>
    protected BaseCapAttribute(T max)
    {
        _minSet = false;
        Min = default;
        Max = max;
    }

    /// <inheritdoc/>
    protected internal override object CreateParameterValue(
        ParameterInfo param,
        MethodBase method,
        OrderedDictionary args,
        RunnerOptions localOptions
    )
    {
        ArgumentGuard.ThrowIfNull(param, method, args, localOptions);

        if (localOptions.Gen.Supports(param.ParameterType))
        {
            if (param.ParameterType == typeof(T))
            {
                if (_minSet)
                {
                    return localOptions.Gen.Next(Min, Max);
                }
                else
                {
                    return localOptions.Gen.Next(Max);
                }
            }
            else
            {
                throw new ToolException(
                    $"Provided min & max of type '{typeof(T)}' must explicitly match '{param}'."
                );
            }
        }
        else
        {
            throw new ToolException($"'{param}' not a support value type for the value generator.");
        }
    }
}
