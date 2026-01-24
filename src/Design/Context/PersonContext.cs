using CreateAndFake.Design.Data;
using CreateAndFake.Design.Randomization;

namespace CreateAndFake.Design.Context;

/// <summary>Bundled random values for a person.</summary>
/// <inheritdoc/>
public sealed class PersonContext(IRandom gen) : BaseDataContext(gen)
{
    /// <summary>Name fragment.</summary>
    private readonly Lazy<string> //.
        _firstName = new(() => gen.NextItem(NameData.Values)),
        _middleName = new(() => gen.NextItem(NameData.Values)),
        _lastName = new(() => gen.NextItem(NameData.Values));

    /// <summary>First name for the person.</summary>
    public string FirstName => _firstName.Value;

    /// <summary>Middle name for the person.</summary>
    public string MiddleName => _middleName.Value;

    /// <summary>Last name for the person.</summary>
    public string LastName => _lastName.Value;

    /// <summary>First and last name for the person.</summary>
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>Initials for the person's name.</summary>
    public string Initials => $"{FirstName[0]}{MiddleName[0]}{LastName[0]}";
}
