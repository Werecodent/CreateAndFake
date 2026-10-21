using System.Collections.Specialized;
using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Extensions;
using Werecodent.CreateAndFake.RunnerTool;
using Werecodent.CreateAndFake.RunnerTool.Attributes;

namespace Werecodent.CreateAndFake.xUnit.v2;

/// <inheritdoc/>
public sealed class StubAttribute : BaseStubAttribute
{
    /// <inheritdoc/>
    protected override object? CreateParameterValue(
        ParameterInfo param,
        MethodBase method,
        OrderedDictionary args,
        RunnerOptions localOptions
    )
    {
        ArgumentGuard.ThrowIfNull(param, localOptions);

        if (param.ParameterType.Inherits<Type>())
        {
            return localOptions.Faker.Stub(param.ParameterType, typeof(IReflectableType)).Dummy;
        }
        else
        {
            return base.CreateParameterValue(param, method, args, localOptions);
        }
    }
}
