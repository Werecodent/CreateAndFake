using CreateAndFake.AsserterTool.Categories;
using CreateAndFake.Design.Types;

namespace CreateAndFake.AsserterTool;

/// <inheritdoc cref="IAsserter"/>
public partial class Asserter : IAsserterType
{
#pragma warning disable CA1716 // Matches existing usage.

    /// <inheritdoc/>
    public virtual void Inherits<TChild>(Type? type, string? details = null)
    {
        Inherits<TChild>(type, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void Inherits<TChild>(
        Type? type,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (!type.Inherits<TChild>())
        {
            throw new AssertException(
                $"'{GenericConverter.ExpandName(type)}' does not inherit '{GenericConverter.ExpandName<TChild>()}'.",
                details,
                localOptions.Gen.InitialSeed
            );
        }
    }

    /// <inheritdoc/>
    public virtual void Inherits(Type? child, Type? type, string? details = null)
    {
        Inherits(child, type, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void Inherits(
        Type? child,
        Type? type,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (!type.Inherits(child))
        {
            throw new AssertException(
                $"'{GenericConverter.ExpandName(type)}' does not inherit '{GenericConverter.ExpandName(child)}'.",
                details,
                localOptions.Gen.InitialSeed
            );
        }
    }

#pragma warning restore CA1716

    /// <inheritdoc/>
    public virtual void InheritedBy<TParent>(Type? type, string? details = null)
    {
        InheritedBy<TParent>(type, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void InheritedBy<TParent>(
        Type? type,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (!type.IsInheritedBy<TParent>())
        {
            throw new AssertException(
                $"'{GenericConverter.ExpandName<TParent>()}' does not inherit '{GenericConverter.ExpandName(type)}'.",
                details,
                localOptions.Gen.InitialSeed
            );
        }
    }

    /// <inheritdoc/>
    public virtual void InheritedBy(Type? parent, Type? type, string? details = null)
    {
        InheritedBy(parent, type, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void InheritedBy(
        Type? parent,
        Type? type,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (!type.IsInheritedBy(parent))
        {
            throw new AssertException(
                $"'{GenericConverter.ExpandName(parent)}' does not inherit '{GenericConverter.ExpandName(type)}'.",
                details,
                localOptions.Gen.InitialSeed
            );
        }
    }
}
