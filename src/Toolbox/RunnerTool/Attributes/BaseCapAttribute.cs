using System.Collections.Specialized;
using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Randomization;

namespace Werecodent.CreateAndFake.RunnerTool.Attributes;

/// <summary>Flag to create the attached value within a range.</summary>
public abstract class BaseCapAttribute : ParameterHintAttribute
{
    private readonly bool _minSet;

    /// <summary>Lower boundary for the generated value.</summary>
    public object Min { get; }

    /// <summary>Upper boundary for the generated value.</summary>
    public object Max { get; }

    /// <summary>
    ///     Flag to create the attached value specifically between <paramref name="min"/> and <paramref name="max"/>.
    /// </summary>
    /// <param name="min"><inheritdoc cref="Min" path="/summary"/> (Inclusive)</param>
    /// <param name="max"><inheritdoc cref="Max" path="/summary"/> (Inclusive)</param>
    /// <seealso cref="IRandom.Next{T}(T,T)"/>
    protected BaseCapAttribute(object min, object max)
    {
        _minSet = true;
        Min = min;
        Max = max;
    }

    /// <summary>Flag to create the attached value specifically below <paramref name="max"/>.</summary>
    /// <param name="max"><inheritdoc cref="Max" path="/summary"/> (Exclusive)</param>
    /// <seealso cref="IRandom.Next{T}(T)"/>
    protected BaseCapAttribute(object max)
    {
        _minSet = false;
        Min = 0;
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
            Type minType = Min.GetType();
            Type maxType = Max.GetType();

            if (_minSet)
            {
                if (minType == maxType && maxType == param.ParameterType)
                {
                    return localOptions.Gen.Next(
                        param.ParameterType,
                        (IComparable)Min,
                        (IComparable)Max
                    );
                }
                else
                {
                    throw new ToolException(
                        $"Provided min '{Min}' of type '{minType}' & max '{Max}' "
                            + $"of type '{maxType}' must explicitly match '{param}'."
                    );
                }
            }
            else
            {
                return localOptions.Gen.Next(param.ParameterType, (IComparable)Max);
            }
        }
        else
        {
            throw new ToolException($"'{param}' not a support value type for the value generator.");
        }
    }
}
