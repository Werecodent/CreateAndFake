using System.Collections.Frozen;
using CreateAndFake.Design.Context;

namespace CreateAndFake.Design.Randomization;

/// <summary>Collects random predefined values to use for data generation.</summary>
/// <param name="gen"><inheritdoc cref="IRandom" path="/summary"/></param>
public sealed class DataRandom(IRandom gen)
{
    /// <summary>Supported searchable property names for values.</summary>
    private static readonly FrozenDictionary<string, Func<DataRandom, string>> _Matcher =
        new Dictionary<string, Func<DataRandom, string>>()
        {
            { "FIRSTNAME", gen => gen.Person.FirstName },
            { "MIDDLENAME", gen => gen.Person.MiddleName },
            { "LASTNAME", gen => gen.Person.LastName },
            { "FULLNAME", gen => gen.Person.FullName },
            { "INITIALS", gen => gen.Person.Initials },
        }.ToFrozenDictionary();

    /// <summary>All searchable names.</summary>
    internal static IEnumerable<string> SupportedProperties { get; } = _Matcher.Keys.ToFrozenSet();

    /// <inheritdoc cref="Person"/>
    private readonly Lazy<PersonContext> _person = new(() => new PersonContext(gen));

    /// <inheritdoc cref="PersonContext"/>
    public PersonContext Person => _person.Value;

    /// <summary>
    ///     Searches for a value representing the identifying <paramref name="name"/>.
    /// </summary>
    /// <param name="name">Name to find a value for.</param>
    /// <returns>
    ///     The value representing <paramref name="name"/> if found,
    ///     <see langword="null"/> otherwise.
    /// </returns>
    public string? Find(string? name)
    {
        return _Matcher.TryGetValue(ToUpperOnly(name), out Func<DataRandom, string>? finder)
            ? finder.Invoke(this)
            : null;
    }

    /// <summary>Converts <paramref name="value"/> to uppercase letters only.</summary>
    /// <param name="value">Text to convert.</param>
    /// <returns>The uppercase converted text.</returns>
    private static string ToUpperOnly(string? value)
    {
        return string.Concat(value?.ToUpperInvariant().Where(c => c is >= 'A' and <= 'Z') ?? []);
    }
}
