namespace CreateAndFake.AsserterTool.Categories;

/// <summary>Handles common type test scenarios.</summary>
public interface ITypeAsserter
{
#pragma warning disable CA1716 // Matches existing usage.

    /// <inheritdoc cref="Inherits{T}(Type,AsserterMod,string)"/>
    void Inherits<TChild>(Type? type, string? details = null);

    /// <summary>Verifies <see cref="Type"/> inherits <typeparamref name="TChild"/>.</summary>
    /// <typeparam name="TChild">Expected child of <see cref="Type"/>.</typeparam>
    /// <param name="type">Type to run assertion checks with.</param>
    /// <inheritdoc cref="IObjectAsserter.Is(object,object,AsserterMod,string)"/>
    void Inherits<TChild>(Type? type, AsserterMod? optionConfiguration, string? details = null);

    /// <inheritdoc cref="Inherits(Type,Type,AsserterMod,string)"/>
    void Inherits(Type? child, Type? type, string? details = null);

    /// <summary>Verifies <see cref="Type"/> inherits <paramref name="child"/>.</summary>
    /// <param name="child">Expected child of <see cref="Type"/>.</param>
    /// <inheritdoc cref="Inherits{T}(Type,AsserterMod,string)"/>
    void Inherits(
        Type? child,
        Type? type,
        AsserterMod? optionConfiguration,
        string? details = null
    );

#pragma warning restore CA1716

    /// <inheritdoc cref="InheritedBy{T}(Type,AsserterMod,string)"/>
    void InheritedBy<TParent>(Type? type, string? details = null);

    /// <summary>Verifies <typeparamref name="TParent"/> inherits <see cref="Type"/>.</summary>
    /// <typeparam name="TParent">Expected parent of <see cref="Type"/>.</typeparam>
    /// <inheritdoc cref="Inherits{T}(Type,AsserterMod,string)"/>
    void InheritedBy<TParent>(Type? type, AsserterMod? optionConfiguration, string? details = null);

    /// <inheritdoc cref="InheritedBy(Type,Type,AsserterMod,string)"/>
    void InheritedBy(Type? parent, Type? type, string? details = null);

    /// <summary>Verifies <paramref name="parent"/> inherits <see cref="Type"/>.</summary>
    /// <param name="parent">Expected parent of <see cref="Type"/>.</param>
    /// <inheritdoc cref="InheritedBy{T}(Type,AsserterMod,string)"/>
    void InheritedBy(
        Type? parent,
        Type? type,
        AsserterMod? optionConfiguration,
        string? details = null
    );
}
