using System.Collections.Frozen;
using System.Reflection;

namespace CreateAndFake.Design.Types;

/// <inheritdoc/>
public sealed class FieldScanner(Type? type) : MemberScanner<FieldInfo>(type)
{
    /// <inheritdoc/>
    public override IEnumerable<FieldInfo> All { get; } = FindAllFields(type).ToFrozenSet();

    /// <inheritdoc/>
    public override IEnumerable<FieldInfo> OnlyPublic => All.Where(f => f.IsPublic);

    /// <inheritdoc/>
    public override IEnumerable<FieldInfo> PublicOrInternal =>
        All.Where(f => f.IsPublic || f.IsAssembly);

    /// <summary>
    ///     The <see langword="public"/> and <see langword="internal"/>
    ///     instance fields on the <see cref="MemberScanner{T}.SupportedType"/> that can be written to.
    /// </summary>
    /// <inheritdoc cref="MemberScanner{T}.Visible"/>
    public IEnumerable<FieldInfo> Writable => FindWritable(Assembly.GetCallingAssembly().GetName());

    /// <summary>
    ///     Finds <see langword="public"/> and <see langword="internal"/>
    ///     instance fields on the <see cref="MemberScanner{T}.SupportedType"/> that can be written to.
    /// </summary>
    /// <inheritdoc cref="MemberScanner{T}.FindVisible(AssemblyName)"/>
    internal IEnumerable<FieldInfo> FindWritable(AssemblyName assembly)
    {
        return FindVisible(assembly).Where(f => !f.IsInitOnly && !f.IsLiteral);
    }

    /// <summary>Finds all instance fields on the <paramref name="type"/>.</summary>
    /// <param name="type">The <see cref="Type"/> to find fields on.</param>
    /// <returns>All found fields on the <see cref="Type"/>.</returns>
    /// <remarks>The <see langword="private"/> fields in inherited <see cref="Type"/>s are included.</remarks>
    private static IEnumerable<FieldInfo> FindAllFields(Type? type)
    {
        if (type == null)
        {
            yield break;
        }

        foreach (
            FieldInfo field in type.GetFields(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            )
        )
        {
            yield return field;
        }

        Type? currentType = type.BaseType;
        HashSet<Type> completedTypes = [type];
        while (currentType != null && completedTypes.Add(currentType))
        {
            foreach (
                FieldInfo field in currentType
                    .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Where(f => f.IsPrivate)
            )
            {
                yield return field;
            }
            currentType = currentType.BaseType;
        }
    }
}
