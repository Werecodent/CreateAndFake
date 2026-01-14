namespace CreateAndFake.Design.Content;

/// <summary>Associates the class with a specific type.</summary>
public interface ITypeSupporter
{
    /// <summary>Specific type handled by <see langword="this"/>.</summary>
    Type SupportedType { get; }
}
