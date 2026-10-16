using System.Reflection;
using NUnit.Framework.Internal;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.RandomizerTool.Engine;

namespace Werecodent.CreateAndFake.NUnit.v3;

/// <inheritdoc/>
public sealed class MethodWrapperCreateHint : CreateHint<MethodWrapper>
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)CreatePriority.ObjectHint + 1;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => [];

    /// <inheritdoc/>
    protected override MethodWrapper Create(IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

        MethodInfo method = randomizer.Create<MethodInfo>();
        return new MethodWrapper(method.DeclaringType, method);
    }
}
