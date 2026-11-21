using System.Collections.Specialized;
using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.FakerTool;

namespace Werecodent.CreateAndFake.RunnerTool.Attributes;

/// <summary>Flag to create the attached <see langword="object"/> as a stub.</summary>
/// <seealso cref="IRunner.CreateFor(MethodBase,CancellationToken,RunnerMod)"/>
/// <seealso cref="IFaker.Stub(Type,IEnumerable{Type})"/>
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

        if (GenericConverter.AsGenericBase(param.ParameterType) == typeof(Fake<>))
        {
            return Activator.CreateInstance(
                param.ParameterType,
                localOptions.Faker.Stub(param.ParameterType.GetGenericArguments()[0])
            );
        }
        else
        {
            return localOptions.Faker.Stub(param.ParameterType).Dummy;
        }
    }
}
