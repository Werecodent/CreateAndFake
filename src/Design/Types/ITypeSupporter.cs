namespace Werecodent.CreateAndFake.Design.Types;

/// <summary>Provides a <see cref="Type"/> that the behavior is relevant for.</summary>
public interface ITypeSupporter
{
    /// <summary>Specific <see cref="Type"/> <see langword="this"/> instance can handle.</summary>
    Type? SupportedType { get; }
}
