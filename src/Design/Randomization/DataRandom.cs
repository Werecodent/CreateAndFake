using System.Collections.Frozen;
using System.Reflection;
using CreateAndFake.Design.Context;
using CreateAndFake.Design.Extensions;

namespace CreateAndFake.Design.Randomization;

/// <summary>Collects random predefined values to use for data generation.</summary>
/// <param name="gen"><inheritdoc cref="IRandom" path="/summary"/></param>
public sealed class DataRandom(IRandom gen)
{
    /// <summary>Supported searchable names for values.</summary>
    private static readonly FrozenDictionary<string, Func<DataRandom, string>> _Matcher =
        new Dictionary<string, Func<DataRandom, string>>()
        {
            { "FIRSTNAME", dat => dat.Person.FirstName },
            { "MIDDLENAME", dat => dat.Person.MiddleName },
            { "LASTNAME", dat => dat.Person.LastName },
            { "FULLNAME", dat => dat.Person.FullName },
            { "INITIALS", dat => dat.Person.Initials },
        }.ToFrozenDictionary();

    /// <summary>All searchable names.</summary>
    internal static IEnumerable<string> SupportedProperties { get; } = _Matcher.Keys.ToFrozenSet();

    /// <inheritdoc cref="Person"/>
    private readonly Lazy<PersonContext> _person = new(() => new PersonContext(gen));

    /// <inheritdoc cref="PersonContext"/>
    public PersonContext Person => _person.Value;

    /// <summary>
    ///     Searches for an identifying value representing the <paramref name="property"/>.
    /// </summary>
    /// <param name="property">Property with the name to find a value for.</param>
    /// <returns>
    ///     A value for the <paramref name="property"/> if found, <see langword="null"/> otherwise.
    /// </returns>
    public string? Find(PropertyInfo property)
    {
        ArgumentGuard.ThrowIfNull(property);
        return property.PropertyType.IsInheritedBy<string>() ? Find(property.Name) : null;
    }

    /// <summary>
    ///     Searches for an identifying value representing the <paramref name="field"/>.
    /// </summary>
    /// <param name="field">Field with the name to find a value for.</param>
    /// <returns>
    ///     A value for the <paramref name="field"/> if found, <see langword="null"/> otherwise.
    /// </returns>
    public string? Find(FieldInfo field)
    {
        ArgumentGuard.ThrowIfNull(field);
        return field.FieldType.IsInheritedBy<string>() ? Find(field.Name) : null;
    }

    /// <summary>
    ///     Searches for an identifying value representing the <paramref name="name"/>.
    /// </summary>
    /// <param name="name">Name to find a value for.</param>
    /// <returns>
    ///     A value for the <paramref name="name"/> if found, <see langword="null"/> otherwise.
    /// </returns>
    public string? Find(string? name)
    {
        return _Matcher.TryGetValue(ToUpperOnly(name), out Func<DataRandom, string>? finder)
            ? finder.Invoke(this)
            : null;
    }

    /// <summary>Converts the <paramref name="value"/> to uppercase letters only.</summary>
    /// <param name="value">Text to convert.</param>
    /// <returns>The uppercase equivalent of the <paramref name="value"/>'s letters.</returns>
    private static string ToUpperOnly(string? value)
    {
        return string.Concat(value?.ToUpperInvariant().Where(c => c is >= 'A' and <= 'Z') ?? []);
    }
}
