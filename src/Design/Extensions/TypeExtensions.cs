using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Design.Types;

namespace CreateAndFake.Design.Extensions;

/// <summary>
///     Extends the <see cref="Type"/> <see langword="class"/> with inheritance methods.
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    ///     Checks if <typeparamref name="T"/> is a base <see langword="class"/> or
    ///     <see langword="interface"/> for <see langword="this"/> <see cref="Type"/>.
    /// </summary>
    /// <typeparam name="T">
    ///     Potential parent <see cref="Type"/> being inherited by the
    ///     <paramref name="child"/> <see cref="Type"/> (<see langword="this"/>).
    /// </typeparam>
    /// <param name="child">
    ///     The <see cref="Type"/> potentially inheriting <typeparamref name="T"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if <see langword="this"/> <see cref="Type"/>
    ///     inherits <typeparamref name="T"/>, <see langword="false"/> otherwise.
    /// </returns>
    /// <inheritdoc cref="Inherits"/>
    public static bool Inherits<T>([NotNullWhen(true)] this Type? child)
    {
        if (child == null)
        {
            return false;
        }
        else if (child.IsGenericTypeDefinition)
        {
            return TypeDescriber.For(child).Inherits<T>();
        }
        else
        {
            return typeof(T).IsAssignableFrom(child);
        }
    }

    /// <summary>
    ///     Checks if the <paramref name="parent"/> is a base <see langword="class"/>
    ///     or <see langword="interface"/> for <see langword="this"/> <see cref="Type"/>.
    /// </summary>
    /// <param name="child">
    ///     <see cref="Type"/> potentially inheriting the <paramref name="parent"/>.
    /// </param>
    /// <param name="parent">
    ///     <see cref="Type"/> potentially inherited by the
    ///     <paramref name="child"/> <see cref="Type"/> (<see langword="this"/>).
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if <see langword="this"/> <see cref="Type"/> inherits the
    ///     <paramref name="parent"/> <see cref="Type"/>, <see langword="false"/> otherwise.
    /// </returns>
    /// <remarks>Supports generic <see cref="Type"/> definitions.</remarks>
    /// <seealso cref="TypeDescriber.Inherits{T}"/>
    public static bool Inherits(
        [NotNullWhen(true)] this Type? child,
        [NotNullWhen(true)] Type? parent
    )
    {
        if (child == null || parent == null)
        {
            return false;
        }
        else if (child.IsGenericTypeDefinition || parent.IsGenericTypeDefinition)
        {
            return TypeDescriber.For(child).Inherits(parent);
        }
        else
        {
            return parent.IsAssignableFrom(child);
        }
    }

    /// <summary>
    ///     Checks if <see langword="this"/> <see cref="Type"/> is a base
    ///     <see langword="class"/> or <see langword="interface"/> for <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Potential child <see cref="Type"/> inheriting the
    ///     <paramref name="parent"/> <see cref="Type"/> (<see langword="this"/>).
    /// </typeparam>
    /// <param name="parent">
    ///     <see cref="Type"/> potentially inherited by <typeparamref name="T"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if <typeparamref name="T"/> inherits
    ///     <see langword="this"/> <see cref="Type"/>, <see langword="false"/> otherwise.
    /// </returns>
    /// <inheritdoc cref="IsInheritedBy(Type,Type)"/>
    public static bool IsInheritedBy<T>([NotNullWhen(true)] this Type? parent)
    {
        if (parent == null)
        {
            return false;
        }
        else if (parent.IsGenericTypeDefinition)
        {
            return TypeDescriber.For<T>().Inherits(parent);
        }
        else
        {
            return parent.IsAssignableFrom(typeof(T));
        }
    }

    /// <summary>
    ///     Checks if <see langword="this"/> <see cref="Type"/> is a base <see langword="class"/>
    ///     or <see langword="interface"/> for the <paramref name="child"/>.
    /// </summary>
    /// <param name="parent">
    ///     <see cref="Type"/> potentially inherited by the <paramref name="child"/>.
    /// </param>
    /// <param name="child">
    ///     <see cref="Type"/> potentially inheriting the
    ///     <paramref name="parent"/> <see cref="Type"/> (<see langword="this"/>).
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <paramref name="child"/> <see cref="Type"/> inherits
    ///     <see langword="this"/> <see cref="Type"/>, <see langword="false"/> otherwise.
    /// </returns>
    /// <inheritdoc cref="Inherits(Type,Type)"/>
    public static bool IsInheritedBy(
        [NotNullWhen(true)] this Type? parent,
        [NotNullWhen(true)] Type? child
    )
    {
        if (child == null || parent == null)
        {
            return false;
        }
        else if (child.IsGenericTypeDefinition || parent.IsGenericTypeDefinition)
        {
            return TypeDescriber.For(child).Inherits(parent);
        }
        else
        {
            return parent.IsAssignableFrom(child);
        }
    }
}
