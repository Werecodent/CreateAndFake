using System.Collections.Frozen;
using System.Reflection;

namespace CreateAndFake.Design.Types;

/// <inheritdoc/>
public sealed class PropertyScanner(Type? type) : MemberScanner<PropertyInfo>(type)
{
    /// <inheritdoc/>
    public override IEnumerable<PropertyInfo> All { get; } = FindAllProperties(type).ToFrozenSet();

    /// <inheritdoc/>
    public override IEnumerable<PropertyInfo> OnlyPublic => SupportedType?.GetProperties() ?? [];

    /// <inheritdoc/>
    public override IEnumerable<PropertyInfo> PublicOrInternal =>
        All.Where(p =>
        {
            MethodInfo? getMethod = p.GetGetMethod(true);
            MethodInfo? setMethod = p.GetSetMethod(true);
            return (getMethod != null && (getMethod.IsPublic || getMethod.IsAssembly))
                || (setMethod != null && (setMethod.IsPublic || setMethod.IsAssembly));
        });

    /// <summary>
    ///     The readable <see langword="public"/> and <see langword="internal"/> instance
    ///     properties on the <see cref="MemberScanner{T}.SupportedType"/> that can be written to.
    /// </summary>
    /// <inheritdoc cref="MemberScanner{T}.Visible"/>
    public IEnumerable<PropertyInfo> SetAndGetable =>
        FindSetAndGetable(Assembly.GetCallingAssembly().GetName());

    /// <summary>
    ///     The <see langword="public"/> and <see langword="internal"/> instance properties
    ///     on the <see cref="MemberScanner{T}.SupportedType"/> that can be written to.
    /// </summary>
    /// <inheritdoc cref="MemberScanner{T}.Visible"/>
    public IEnumerable<PropertyInfo> Settable =>
        FindSettable(Assembly.GetCallingAssembly().GetName());

    /// <inheritdoc cref="FindSettable(AssemblyName)"/>
    internal IEnumerable<PropertyInfo> FindSetAndGetable(AssemblyName assembly)
    {
        bool nonPublic = TypeHelper.InternalsAreVisible(SupportedType, assembly);
        return FindSettable(assembly)
            .Where(p =>
            {
                MethodInfo? getMethod = p.GetGetMethod(nonPublic);
                return getMethod != null && (getMethod.IsPublic || getMethod.IsAssembly);
            });
    }

    /// <summary>
    ///     Finds <see langword="public"/> and <see langword="internal"/> instance properties
    ///     on the <see cref="MemberScanner{T}.SupportedType"/> that can be written to.
    /// </summary>
    /// <inheritdoc cref="MemberScanner{T}.FindVisible(AssemblyName)"/>
    internal IEnumerable<PropertyInfo> FindSettable(AssemblyName assembly)
    {
        bool nonPublic = TypeHelper.InternalsAreVisible(SupportedType, assembly);
        return All.Where(p =>
        {
            MethodInfo? setMethod = p.GetSetMethod(nonPublic);
            return setMethod != null && (setMethod.IsPublic || setMethod.IsAssembly);
        });
    }

    /// <summary>Finds all instance properties on the <paramref name="type"/>.</summary>
    /// <param name="type">The <see cref="Type"/> to find properties on.</param>
    /// <returns>All found properties on the <see cref="Type"/>.</returns>
    /// <remarks>The <see langword="private"/> properties in inherited <see cref="Type"/>s are included.</remarks>
    private static IEnumerable<PropertyInfo> FindAllProperties(Type? type)
    {
        if (type == null)
        {
            yield break;
        }

        foreach (
            PropertyInfo prop in type.GetProperties(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
            )
        )
        {
            yield return prop;
        }

        Type? currentType = type;
        HashSet<Type> completedTypes = [];
        while (currentType != null && completedTypes.Add(currentType))
        {
            foreach (
                PropertyInfo prop in currentType
                    .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Where(p =>
                        p.GetSetMethod(true)?.IsPrivate != false
                        && p.GetGetMethod(true)?.IsPrivate != false
                    )
            )
            {
                yield return prop;
            }
            currentType = currentType.BaseType;
        }
    }
}
