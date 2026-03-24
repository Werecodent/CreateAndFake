using System.Collections.Frozen;
using System.Reflection;
using CreateAndFake.Design.Extensions;

namespace CreateAndFake.Design.Types;

/// <summary>Finds factories on the <see cref="MemberScanner{T}.SupportedType"/>.</summary>
/// <inheritdoc/>
public sealed class FactoryScanner(Type? type) : MemberScanner<MethodInfo>(type)
{
    /// <summary>
    ///     All <see langword="static"/> methods that create the <see cref="MemberScanner{T}.SupportedType"/>.
    /// </summary>
    public override IEnumerable<MethodInfo> All { get; } = FindAllFactories(type).ToFrozenSet();

    /// <summary>
    ///     The <see langword="public"/> <see langword="static"/>
    ///     methods that create the <see cref="MemberScanner{T}.SupportedType"/>.
    /// </summary>
    public override IEnumerable<MethodInfo> OnlyPublic =>
        SupportedType
            ?.GetMethods(BindingFlags.Static | BindingFlags.Public)
            .Where(m => m.ReturnType.Inherits(SupportedType))
        ?? [];

    /// <summary>
    ///     The <see langword="public"/> or <see langword="internal"/> <see langword="static"/>
    ///     methods that create the <see cref="MemberScanner{T}.SupportedType"/>.
    /// </summary>
    public override IEnumerable<MethodInfo> PublicOrInternal =>
        All.Where(m => m.IsPublic || m.IsAssembly);

    /// <summary>Finds all <see langword="static"/> methods that create the <paramref name="type"/>.</summary>
    /// <param name="type">The <see cref="Type"/> to find factories on.</param>
    /// <returns>All found factory methods on the <see cref="Type"/>.</returns>
    private static IEnumerable<MethodInfo> FindAllFactories(Type? type)
    {
        return type?.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m =>
                    (GenericTypeConverter.AsGenericBase(m.ReturnType) ?? m.ReturnType) == type
                )
            ?? [];
    }
}
