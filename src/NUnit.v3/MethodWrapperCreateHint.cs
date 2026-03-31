using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.RandomizerTool.Engine;
using NUnit.Framework.Internal;

namespace CreateAndFake.NUnit.v3;

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
