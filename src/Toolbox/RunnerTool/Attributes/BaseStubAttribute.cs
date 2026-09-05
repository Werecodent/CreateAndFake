using System.Collections.Specialized;
using System.Reflection;
using Werecodent.CreateAndFake.Design;

namespace Werecodent.CreateAndFake.RunnerTool.Attributes;

/// <summary>Flag to create the attached <see langword="object"/> as a stub.</summary>
/// <seealso cref="IRunner.CreateFor(MethodBase,CancellationToken,RunnerMod)"/>
/// <seealso cref="FakerTool.IFaker.Stub(Type,IEnumerable{Type})"/>
public abstract class BaseStubAttribute : ParameterHintAttribute
{
    /// <inheritdoc/>
    protected internal override object? CreateParameterValue(
        ParameterInfo param,
        MethodBase method,
        OrderedDictionary args,
        RunnerOptions localOptions
    )
    {
        ArgumentGuard.ThrowIfNull(param, localOptions);

        if (localOptions.InheritIReflectableTypeOnFakedType && param.ParameterType.Inherits<Type>())
        {
            return localOptions.Faker.Stub(param.ParameterType, typeof(IReflectableType)).Dummy;
        }
        else
        {
            return localOptions.Faker.Stub(param.ParameterType).Dummy;
        }
    }
}
