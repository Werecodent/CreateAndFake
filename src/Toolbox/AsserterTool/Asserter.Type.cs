using CreateAndFake.AsserterTool.Categories;

namespace CreateAndFake.AsserterTool;

/// <inheritdoc cref="IAsserter"/>
public partial class Asserter : ITypeAsserter
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
                $"'{ExpandTypeName(type)}' does not inherit '{ExpandTypeName(typeof(TChild))}'.",
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
                $"'{ExpandTypeName(type)}' does not inherit '{ExpandTypeName(child)}'.",
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
                $"'{ExpandTypeName(typeof(TParent))}' does not inherit '{ExpandTypeName(type)}'.",
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
                $"'{ExpandTypeName(parent)}' does not inherit '{ExpandTypeName(type)}'.",
                details,
                localOptions.Gen.InitialSeed
            );
        }
    }
}
