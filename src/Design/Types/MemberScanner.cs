using System.Reflection;

namespace CreateAndFake.Design.Types;

/// <summary>Finds <typeparamref name="T"/> members on the <see cref="SupportedType"/>.</summary>
/// <typeparam name="T">The member <see cref="Type"/> being found on the <see cref="SupportedType"/>.</typeparam>
/// <param name="type"><inheritdoc cref="SupportedType" path="/summary"/></param>
public abstract class MemberScanner<T>(Type? type) : ITypeSupporter
    where T : MemberInfo
{
    /// <inheritdoc/>
    public Type? SupportedType { get; } = type;

    /// <summary>All instance <typeparamref name="T"/> members on the <see cref="SupportedType"/>.</summary>
    /// <remarks>
    ///     The <see langword="private"/> <typeparamref name="T"/>
    ///     members in inherited <see cref="Type"/>s are included.
    /// </remarks>
    public abstract IEnumerable<T> All { get; }

    /// <summary>
    ///     The <see langword="public"/> instance <typeparamref name="T"/> members on the <see cref="SupportedType"/>.
    /// </summary>
    /// <remarks>Includes inherited <typeparamref name="T"/> members.</remarks>
    public abstract IEnumerable<T> OnlyPublic { get; }

    /// <summary>
    ///     The <see langword="public"/> and <see langword="internal"/> instance
    ///     <typeparamref name="T"/> members on the <see cref="SupportedType"/>.
    /// </summary>
    /// <remarks>Includes inherited <typeparamref name="T"/> members.</remarks>
    public abstract IEnumerable<T> PublicOrInternal { get; }

    /// <remarks>
    ///     Finds <see langword="internal"/> <typeparamref name="T"/> members only if
    ///     they are visible to the calling method's <see cref="Assembly"/>. Mark an
    ///     <see cref="Assembly"/> with <c>InternalsVisibleTo("CreateAndFake")</c> to
    ///     access its  <see langword="internal"/> properties for the test framework.
    /// </remarks>
    /// <inheritdoc cref="PublicOrInternal"/>
    public IEnumerable<T> Visible => FindVisible(Assembly.GetCallingAssembly().GetName());

    /// <summary>
    ///     Finds <see langword="public"/> and <see langword="internal"/>
    ///     instance properties on the <see cref="SupportedType"/>.
    /// </summary>
    /// <param name="assembly">Name of the <see cref="Assembly"/> to determine visibility for.</param>
    /// <returns>All found properties on the <see cref="Type"/>.</returns>
    protected internal IEnumerable<T> FindVisible(AssemblyName assembly)
    {
        return ScopeChecker.InternalsAreVisible(SupportedType, assembly)
            ? PublicOrInternal
            : OnlyPublic;
    }
}
