using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;

namespace Werecodent.CreateAndFake.Design.Types;

/// <summary>Provides common <see cref="Type"/> patterns for verifying <see cref="Assembly"/> access.</summary>
public static class ScopeChecker
{
    /// <summary>Determines if <typeparamref name="T"/> is usable in the <paramref name="assembly"/>.</summary>
    /// <typeparam name="T">The <see cref="Type"/> to verify visibility for.</typeparam>
    /// <inheritdoc cref="IsVisible(Type,AssemblyName)"/>
    public static bool IsVisible<T>(AssemblyName assembly)
    {
        return IsVisible(typeof(T), assembly);
    }

    /// <summary>Determines if the <paramref name="type"/> is usable in the <paramref name="assembly"/>.</summary>
    /// <param name="type">The <see cref="Type"/> to verify visibility for.</param>
    /// <param name="assembly">Name of the <see cref="Assembly"/> trying to use the <see cref="Type"/>.</param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="Type"/> is visible to
    ///     the <paramref name="assembly"/>, <see langword="false"/> otherwise.
    /// </returns>
    /// <remarks>
    ///     Mark an <see cref="Assembly"/> with <c>InternalsVisibleTo("CreateAndFake")</c>
    ///     to access its <see langword="internal"/> types for this method.
    /// </remarks>
    public static bool IsVisible(Type? type, AssemblyName assembly)
    {
        return type != null && (type.IsVisible || InternalsAreVisible(type, assembly));
    }

    /// <summary>
    ///     Determines if the <paramref name="type"/>'s <see langword="internal"/>
    ///     members are usable in the <paramref name="assembly"/>.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> to verify visibility for.</param>
    /// <param name="assembly">Name of the <see cref="Assembly"/> to check scope privilege for.</param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="Type"/>'s <see langword="internal"/>s
    ///     are visible to the <paramref name="assembly"/>, <see langword="false"/> otherwise.
    /// </returns>
    /// <remarks>
    ///     Mark an <see cref="Assembly"/> with <c>InternalsVisibleTo("CreateAndFake")</c>
    ///     to enable <see langword="internal"/> members visibility per this method.
    /// </remarks>
    internal static bool InternalsAreVisible(Type? type, AssemblyName assembly)
    {
        ArgumentGuard.ThrowIfNull(assembly);

        Assembly? typeAssembly = type?.Assembly;
        return typeAssembly != null
            && (
                typeAssembly.FullName == assembly.FullName
                || typeAssembly
                    .GetCustomAttributes<InternalsVisibleToAttribute>()
                    .Any(a => a.AssemblyName == assembly.Name)
            );
    }

    /// <summary>
    ///     Finds every <see langword="class"/> and <see langword="struct"/> in the <paramref name="assembly"/>.
    /// </summary>
    /// <param name="assembly"><see cref="Assembly"/> containing the <see cref="Type"/>s to search for.</param>
    /// <returns>Every found <see cref="Type"/> if the <paramref name="assembly"/> loads, none otherwise.</returns>
    public static IEnumerable<Type> FindLoadedSpecificTypes(Assembly? assembly)
    {
        if (assembly == null)
        {
            return [];
        }

        return FindLoadedTypes(assembly)
            .Where(t => t.IsClass || t.IsValueType)
            .Where(t => !t.IsNestedPrivate)
            .Where(t => !t.IsDefined(typeof(CompilerGeneratedAttribute), false));
    }

    /// <summary>Finds every <see cref="Type"/> in the <paramref name="assembly"/>.</summary>
    /// <param name="assembly"><see cref="Assembly"/> containing the <see cref="Type"/>s to search for.</param>
    /// <returns>Every found <see cref="Type"/> if the <paramref name="assembly"/> can load, none otherwise.</returns>
    internal static IEnumerable<Type> FindLoadedTypes(Assembly? assembly)
    {
        try
        {
            return assembly?.GetTypes() ?? Type.EmptyTypes;
        }
        catch
        {
            return Type.EmptyTypes;
        }
    }
}
