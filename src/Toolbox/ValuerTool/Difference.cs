using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.ValuerTool;

/// <summary>Expresses a value difference between two objects.</summary>
public sealed class Difference : IValueEquatable, IDeepCloneable<Difference>
{
    /// <summary>Message stating the difference.</summary>
    private readonly Lazy<string> _message;

    /// <inheritdoc cref="Difference"/>
    /// <param name="expectedType"><see cref="Type"/> of the compared expected object.</param>
    /// <param name="actualType"><see cref="Type"/> of the compared actual object.</param>
    public Difference(Type expectedType, Type? actualType)
    {
        _message = new Lazy<string>(() =>
            $"-> Expected type:<{GenericConverter.ExpandName(expectedType)}>, "
            + $"Actual type:<{GenericConverter.ExpandName(actualType)}>"
        );
    }

    /// <inheritdoc cref="Difference"/>
    /// <param name="expected"><see langword="object"/> compared with <paramref name="actual"/>.</param>
    /// <param name="actual"><see langword="object"/> compared against <paramref name="expected"/>.</param>
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
        ArgumentGuard.ThrowIfNull(member);
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
        ArgumentGuard.ThrowIfNull(access);
        ArgumentGuard.ThrowIfNull(difference);

        _message = new Lazy<string>(() => access + difference);
    }

    /// <inheritdoc cref="Difference"/>
    /// <param name="message"><inheritdoc cref="_message" path="/summary"/></param>
    public Difference(string message)
    {
        _message = new Lazy<string>(() => message);
    }

    /// <inheritdoc/>
    public Difference DeepClone()
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

    /// <summary>Converts <see langword="this"/> to a <see langword="string"/>.</summary>
    /// <returns><see langword="string"/> representation of <see langword="this"/>.</returns>
    public override string ToString()
    {
        return _message.Value;
    }
}
