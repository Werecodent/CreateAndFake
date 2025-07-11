using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Design.Content;

#pragma warning disable IDE0130 // To be included for entire library.

namespace CreateAndFake;

/// <summary>Inheritance methods to extend the <see cref="Type"/> <see langword="class"/>.</summary>
public static class TypeExtensions
{
    /// <summary>
    ///     Attempts to cast <paramref name="type"/> (<see langword="this"/>)
    ///     to its generic <see cref="Type"/> definition.
    /// </summary>
    /// <param name="type"><see cref="Type"/> to cast.</param>
    /// <returns>
    ///     The casted <paramref name="type"/> if generic, <see langword="null"/> otherwise.
    /// </returns>
    public static Type? AsGenericBase(this Type? type)
    {
        return type?.IsGenericType == true ? type.GetGenericTypeDefinition() : null;
    }

    /// <summary>
    ///     Checks if <paramref name="parent"/>
    ///     (<see langword="this"/>) inherits <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    ///     Potential child <see cref="Type"/> of <paramref name="parent"/>.
    /// </typeparam>
    /// <param name="parent">Potential parent of <typeparamref name="T"/>.</param>
    /// <returns>
    ///     <see langword="true"/> if <paramref name="parent"/> inherits <typeparamref name="T"/>,
    ///     <see langword="false"/> otherwise.
    /// </returns>
    public static bool Inherits<T>([NotNullWhen(true)] this Type? parent)
    {
        return TypeDescriber.For(parent).Inherits<T>();
    }

    /// <summary>
    ///     Checks if <paramref name="parent"/>
    ///     (<see langword="this"/>) inherits <paramref name="child"/>.
    /// </summary>
    /// <param name="parent">Potential parent of <paramref name="child"/>.</param>
    /// <param name="child">Potential child of <paramref name="parent"/>.</param>
    /// <returns>
    ///     <see langword="true"/> if <paramref name="parent"/> inherits <paramref name="child"/>,
    ///     <see langword="false"/> otherwise.
    /// </returns>
    public static bool Inherits(
        [NotNullWhen(true)] this Type? parent,
        [NotNullWhen(true)] Type? child
    )
    {
        return TypeDescriber.For(parent).Inherits(child);
    }

    /// <summary>
    ///     Checks if <typeparamref name="T"/> inherits
    ///     <paramref name="child"/> (<see langword="this"/>).
    /// </summary>
    /// <typeparam name="T">
    ///     Potential parent <see cref="Type"/> of <paramref name="child"/>.
    /// </typeparam>
    /// <param name="child">Potential child of <typeparamref name="T"/>.</param>
    /// <returns>
    ///     <see langword="true"/> if <typeparamref name="T"/> inherits <paramref name="child"/>,
    ///     <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsInheritedBy<T>([NotNullWhen(true)] this Type? child)
    {
        return TypeDescriber.For<T>().Inherits(child);
    }

    /// <summary>
    ///     Checks if <paramref name="parent"/> inherits
    ///     <paramref name="child"/> (<see langword="this"/>).
    /// </summary>
    /// <inheritdoc cref="Inherits"/>
    public static bool IsInheritedBy(
        [NotNullWhen(true)] this Type? child,
        [NotNullWhen(true)] Type? parent
    )
    {
        return TypeDescriber.For(parent).Inherits(child);
    }
}

#pragma warning restore IDE0130
