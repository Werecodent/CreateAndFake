using System.Collections.Frozen;
using System.Reflection;

namespace CreateAndFake.Design.Types;

/// <inheritdoc/>
public sealed class MethodScanner(Type? type) : MemberScanner<MethodInfo>(type)
{
    /// <inheritdoc/>
    public override IEnumerable<MethodInfo> All { get; } = FindAllMethods(type).ToFrozenSet();

    /// <inheritdoc/>
    public override IEnumerable<MethodInfo> OnlyPublic => All.Where(m => m.IsPublic);

    /// <inheritdoc/>
    public override IEnumerable<MethodInfo> PublicOrInternal =>
        All.Where(c => c.IsPublic || c.IsAssembly || c.IsFamilyOrAssembly);

    /// <summary>Finds all methods on the <paramref name="type"/>.</summary>
    /// <param name="type">The <see cref="Type"/> to find methods on.</param>
    /// <returns>All found methods on the <see cref="Type"/>.</returns>
    /// <remarks>The <see langword="private"/> methods in inherited <see cref="Type"/>s are included.</remarks>
    private static IEnumerable<MethodInfo> FindAllMethods(Type? type)
    {
        if (type == null)
        {
            yield break;
        }

        foreach (
            MethodInfo method in type.GetMethods(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            )
        )
        {
            if (method.Name != "Finalize")
            {
                yield return method;
            }
        }

        Type? currentType = type;
        HashSet<Type> completedTypes = [typeof(object)];
        while (currentType != null && completedTypes.Add(currentType))
        {
            foreach (
                MethodInfo method in currentType
                    .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Where(m => m.IsPrivate)
            )
            {
                yield return method;
            }
            currentType = currentType.BaseType;
        }
    }
}
