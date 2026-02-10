namespace CreateAndFake.Design.Tooling;

/// <summary>Hint for controlling <see cref="IHintTool{T,T}"/> behavior.</summary>
public interface IToolHint
{
    /// <summary>Determines running order for dynamically loaded hints.</summary>
    /// <remarks>Higher values take precedence.</remarks>
    int EnginePriority { get; }

    /// <summary>Specific <see cref="Type"/>s explicitly supported.</summary>
    /// <remarks>
    ///     Not inclusive and not required. Generic bases might only indicate
    ///     support for the <see cref="Type"/> specified with generics populated.
    /// </remarks>
    IEnumerable<Type> SupportedTypes { get; }
}
