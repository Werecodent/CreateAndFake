using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;

namespace CreateAndFake.Toolbox.ValuerTool;

/// <summary>Expresses a value difference between two objects.</summary>
public sealed class Difference : IValueEquatable, IDeepCloneable
{
    /// <summary>Message stating the difference.</summary>
    private readonly Lazy<string> _message;

    /// <inheritdoc cref="Difference"/>
    /// <param name="expectedType"><c>Type</c> of the compared expected object.</param>
    /// <param name="actualType"><c>Type</c> of the compared actual object.</param>
    public Difference(Type expectedType, Type actualType)
    {
        _message = new Lazy<string>(
            () => $" -> Expected type:<{expectedType}>, Actual type:<{actualType}>");
    }

    /// <inheritdoc cref="Difference"/>
    /// <param name="expected"><c>Object</c> compared with <paramref name="actual"/>.</param>
    /// <param name="actual"><c>Object</c> compared against <paramref name="expected"/>.</param>
    public Difference(object? expected, object? actual)
    {
        _message = new Lazy<string>(() => $" -> Expected:<{expected}>, Actual:<{actual}>");
    }

    /// <inheritdoc cref="Difference"/>
    /// <param name="member">Member where the compared objects differed.</param>
    /// <param name="difference">Found difference for the compared objects.</param>
    public Difference(MemberInfo member, Difference difference)
        : this("." + member?.Name, difference)
    {
        ArgumentGuard.ThrowIfNull(member, nameof(member));
    }

    /// <inheritdoc cref="Difference"/>
    /// <param name="index">Index where the compared objects differed.</param>
    /// <param name="difference">Found difference for the compared objects.</param>
    public Difference(int index, Difference difference)
        : this($"[{index}]", difference) { }

    /// <inheritdoc cref="Difference"/>
    /// <param name="access">Access method where the compared objects differed.</param>
    /// <param name="difference">Found difference for the compared objects.</param>
    public Difference(string access, Difference difference)
    {
        ArgumentGuard.ThrowIfNull(access, nameof(access));
        ArgumentGuard.ThrowIfNull(difference, nameof(difference));

        _message = new Lazy<string>(() => access + difference.ToString());
    }

    /// <inheritdoc cref="Difference"/>
    /// <param name="message"><inheritdoc cref="_message" path="/summary"/></param>
    public Difference(string message)
    {
        _message = new Lazy<string>(() => message);
    }

    /// <inheritdoc/>
    public IDeepCloneable DeepClone()
    {
        return new Difference(_message.Value);
    }

    /// <inheritdoc/>
    public bool ValuesEqual(object? other)
    {
        return other != null
            && GetType() == other.GetType()
            && _message.Value == ((Difference)other)._message.Value;
    }

    /// <inheritdoc/>
    public int GetValueHash()
    {
        return ValueComparer.Use.GetHashCode(_message.Value);
    }

    /// <summary>Converts <c>this</c> to a <c>string</c>.</summary>
    /// <returns><c>string</c> representation of <c>this</c>.</returns>
    public override string ToString()
    {
        return _message.Value;
    }
}
