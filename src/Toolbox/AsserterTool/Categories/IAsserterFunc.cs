namespace Werecodent.CreateAndFake.AsserterTool.Categories;

/// <summary>Handles common delegate test scenarios.</summary>
public interface IAsserterFunc
{
    /// <inheritdoc cref="IAsserterDelegate.HasResult{T}(Delegate,string)"/>
    T HasResult<T>(Func<T>? behavior, string? details = null);

    /// <inheritdoc cref="IAsserterDelegate.HasResult{T}(Delegate,AsserterMod,string)"/>
    T HasResult<T>(Func<T>? behavior, AsserterMod? optionConfiguration, string? details = null);
}
