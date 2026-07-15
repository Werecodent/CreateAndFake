using System.Collections.Frozen;
using System.Reflection;

namespace CreateAndFake.Design.Types;

/// <inheritdoc/>
public sealed class ConstructorScanner(Type? type) : MemberScanner<ConstructorInfo>(type)
{
    /// <remarks></remarks>
    /// <inheritdoc/>
    public override IEnumerable<ConstructorInfo> All { get; } =
        FindAllConstructors(type).ToFrozenSet();

    /// <remarks></remarks>
    /// <inheritdoc/>
    public override IEnumerable<ConstructorInfo> OnlyPublic => All.Where(c => c.IsPublic);

    /// <remarks></remarks>
    /// <inheritdoc/>
    public override IEnumerable<ConstructorInfo> PublicOrInternal =>
        All.Where(c => c.IsPublic || c.IsAssembly);

    /// <summary>Finds all constructors on the <paramref name="type"/>.</summary>
    /// <param name="type">The <see cref="Type"/> to find constructors on.</param>
    /// <returns>All found constructors on the <see cref="Type"/>.</returns>
    private static ConstructorInfo[] FindAllConstructors(Type? type)
    {
        return type?.GetConstructors(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            ) ?? [];
    }
}
