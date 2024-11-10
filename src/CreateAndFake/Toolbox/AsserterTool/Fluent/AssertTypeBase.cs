#pragma warning disable CA1716 // Rename conflicting virtual/interface member: Overriding here should be a rarity.

namespace CreateAndFake.Toolbox.AsserterTool.Fluent;

/// <summary>Handles common <see cref="Type"/> assertion calls.</summary>
/// <param name="type"><inheritdoc cref="Type" path="/summary"/></param>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertTypeBase<T>(AsserterOptions options, Type? type)
    : AssertObjectBase<T>(options, type) where T : AssertTypeBase<T>
{
    /// <summary>Type to run assertion checks with.</summary>
    protected Type? Type { get; } = type;

    /// <summary>Verifies <c>Type</c> inherits <typeparamref name="TChild"/>.</summary>
    /// <typeparam name="TChild">Potential child of <c>Type</c>.</typeparam>
    /// <param name="details">Optional failure details to include.</param>
    /// <returns>Chainer to make additional assertions with.</returns>
    /// <exception cref="AssertException">If <c>Type</c> fails to match the expected behavior.</exception>
    public virtual AssertChainer<T> Inherits<TChild>(string? details = null)
    {
        if (!Type.Inherits<TChild>())
        {
            throw new AssertException(
                $"'{ExpandTypeName(Type)}' does not inherit '{ExpandTypeName(typeof(TChild))}'.",
                details,
                Options.Gen.InitialSeed);
        }
        return ToChainer();
    }

    /// <summary>Verifies <c>Type</c> inherits <paramref name="child"/>.</summary>
    /// <param name="child">Potential child of <c>Type</c>.</param>
    /// <inheritdoc cref="Inherits{T}"/>
    public virtual AssertChainer<T> Inherits(Type? child, string? details = null)
    {
        if (!Type.Inherits(child))
        {
            throw new AssertException(
                $"'{ExpandTypeName(Type)}' does not inherit '{ExpandTypeName(child)}'.",
                details,
                Options.Gen.InitialSeed);
        }
        return ToChainer();
    }

    /// <summary>Verifies <typeparamref name="TParent"/> inherits <c>Type</c>.</summary>
    /// <typeparam name="TParent">Potential parent of <c>Type</c>.</typeparam>
    /// <inheritdoc cref="Inherits{T}"/>
    public virtual AssertChainer<T> InheritedBy<TParent>(string? details = null)
    {
        if (!Type.IsInheritedBy<TParent>())
        {
            throw new AssertException(
                $"'{ExpandTypeName(typeof(TParent))}' does not inherit '{ExpandTypeName(Type)}'.",
                details,
                Options.Gen.InitialSeed);
        }
        return ToChainer();
    }

    /// <summary>Verifies <paramref name="parent"/> inherits <c>Type</c>.</summary>
    /// <param name="parent">Potential parent of <c>Type</c>.</param>
    /// <inheritdoc cref="Inherits{T}"/>
    public virtual AssertChainer<T> InheritedBy(Type? parent, string? details = null)
    {
        if (!Type.IsInheritedBy(parent))
        {
            throw new AssertException(
                $"'{ExpandTypeName(parent)}' does not inherit '{ExpandTypeName(Type)}'.",
                details,
                Options.Gen.InitialSeed);
        }
        return ToChainer();
    }
}

#pragma warning restore CA1716 // Rename conflicting virtual/interface member
