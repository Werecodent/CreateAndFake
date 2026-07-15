using System.Collections.Frozen;
using System.Reflection;

namespace CreateAndFake.Design.Types;

/// <inheritdoc/>
public sealed class StaticMethodScanner(Type? type) : MemberScanner<MethodInfo>(type)
{
    /// <summary>All static methods on the <see cref="MemberScanner{T}.SupportedType"/>.</summary>
    /// <remarks>Does not include inherited static members.</remarks>
    public override IEnumerable<MethodInfo> All { get; } = FindAllMethods(type).ToFrozenSet();

    /// <summary>
    ///     The <see langword="public"/> static methods on the <see cref="MemberScanner{T}.SupportedType"/>.
    /// </summary>
    /// <remarks>Does not include inherited static members.</remarks>
    public override IEnumerable<MethodInfo> OnlyPublic => All.Where(m => m.IsPublic);

    /// <summary>
    ///     The <see langword="public"/> and <see langword="internal"/> static
    ///     methods on the <see cref="MemberScanner{T}.SupportedType"/>.
    /// </summary>
    /// <remarks>Does not include inherited static members.</remarks>
    public override IEnumerable<MethodInfo> PublicOrInternal =>
        All.Where(c => c.IsPublic || c.IsAssembly || c.IsFamilyOrAssembly);

    /// <summary>Finds all static methods on the <paramref name="type"/>.</summary>
    /// <param name="type">The <see cref="Type"/> to find methods on.</param>
    /// <returns>All found static methods on the <see cref="Type"/>.</returns>
    private static MethodInfo[] FindAllMethods(Type? type)
    {
        return type?.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
            ?? [];
    }
}
