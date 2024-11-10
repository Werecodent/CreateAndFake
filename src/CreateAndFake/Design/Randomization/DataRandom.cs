using System.Text;
using CreateAndFake.Design.Context;

namespace CreateAndFake.Design.Randomization;

/// <summary>Collects random predefined values to use for data generation.</summary>
public sealed class DataRandom
{
    /// <summary>Supported searchable property names for values.</summary>
    private static readonly Dictionary<string, Func<DataRandom, string>> _Matcher = new()
    {
        { "FIRSTNAME", gen => gen.Person.FirstName },
        { "MIDDLENAME", gen => gen.Person.MiddleName },
        { "LASTNAME", gen => gen.Person.LastName },
        { "FULLNAME", gen => gen.Person.FullName },
        { "INITIALS", gen => gen.Person.Initials }
    };

    /// <summary>All searchable names.</summary>
    internal static IReadOnlyCollection<string> SupportedProperties => _Matcher.Keys;

    /// <inheritdoc cref="IRandom"/>
    private readonly IRandom _gen;

    /// <inheritdoc cref="Person"/>
    private readonly Lazy<PersonContext> _person;

    /// <inheritdoc cref="PersonContext"/>
    public PersonContext Person => _person.Value;

    /// <inheritdoc cref="DataRandom"/>
    /// <param name="gen"><inheritdoc cref="_gen" path="/summary"/></param>
    internal DataRandom(IRandom gen)
    {
        _gen = gen ?? throw new ArgumentNullException(nameof(gen));
        _person = new(() => new PersonContext(_gen));
    }

    /// <summary>Searches for a value representing the identifying <paramref name="name"/>.</summary>
    /// <param name="name">Name to match a value with.</param>
    /// <returns>The value representing <paramref name="name"/> if found; <c>null</c> otherwise.</returns>
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
        StringBuilder result = new();
        foreach (char c in value?.ToUpperInvariant() ?? "")
        {
            if (c is >= 'A' and <= 'Z')
            {
                _ = result.Append(c);
            }
        }
        return result.ToString();
    }
}
