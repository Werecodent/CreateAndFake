using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Design.Content;

#pragma warning disable IDE0130 // To be included for entire library.

namespace CreateAndFake;

/// <summary>Inheritance methods to extend the <see cref="Type"/> <see langword="class"/>.</summary>
public static class TypeExtensions
{
    /// <summary>
    ///     Checks if <paramref name="child"/>
    ///     (<see langword="this"/>) inherits <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    ///     Potential parent <see cref="Type"/> of <paramref name="child"/>.
    /// </typeparam>
    /// <param name="child">Potential child of <typeparamref name="T"/>.</param>
    /// <returns>
    ///     <see langword="true"/> if <paramref name="child"/> inherits <typeparamref name="T"/>,
    ///     <see langword="false"/> otherwise.
    /// </returns>
    public static bool Inherits<T>([NotNullWhen(true)] this Type? child)
    {
        if (child == null)
        {
            return false;
        }
        else if (child.IsGenericTypeDefinition)
        {
            return InheritanceTracker.For(child).Inherits<T>();
        }
        else
        {
            return typeof(T).IsAssignableFrom(child);
        }
    }

    /// <summary>
    ///     Checks if <paramref name="child"/>
    ///     (<see langword="this"/>) inherits <paramref name="parent"/>.
    /// </summary>
    /// <param name="child">Potential child of <paramref name="parent"/>.</param>
    /// <param name="parent">Potential parent of <paramref name="child"/>.</param>
    /// <returns>
    ///     <see langword="true"/> if <paramref name="child"/> inherits <paramref name="parent"/>,
    ///     <see langword="false"/> otherwise.
    /// </returns>
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
            return InheritanceTracker.For(child).Inherits(parent);
        }
        else
        {
            return parent.IsAssignableFrom(child);
        }
    }

    /// <summary>
    ///     Checks if <typeparamref name="T"/> inherits
    ///     <paramref name="parent"/> (<see langword="this"/>).
    /// </summary>
    /// <typeparam name="T">
    ///     Potential child <see cref="Type"/> of <paramref name="parent"/>.
    /// </typeparam>
    /// <param name="parent">Potential parent of <typeparamref name="T"/>.</param>
    /// <returns>
    ///     <see langword="true"/> if <typeparamref name="T"/> inherits <paramref name="parent"/>,
    ///     <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsInheritedBy<T>([NotNullWhen(true)] this Type? parent)
    {
        if (parent == null)
        {
            return false;
        }
        else if (parent.IsGenericTypeDefinition)
        {
            return InheritanceTracker.For<T>().Inherits(parent);
        }
        else
        {
            return parent.IsAssignableFrom(typeof(T));
        }
    }

    /// <summary>
    ///     Checks if <paramref name="child"/> inherits
    ///     <paramref name="parent"/> (<see langword="this"/>).
    /// </summary>
    /// <inheritdoc cref="Inherits"/>
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
            return InheritanceTracker.For(child).Inherits(parent);
        }
        else
        {
            return parent.IsAssignableFrom(child);
        }
    }
}

#pragma warning restore IDE0130
