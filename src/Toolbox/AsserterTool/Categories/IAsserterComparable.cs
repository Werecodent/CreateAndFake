namespace CreateAndFake.AsserterTool.Categories;

/// <summary>Handles common comparable test scenarios.</summary>
public interface IAsserterComparable
{
    /// <inheritdoc cref="GreaterThan(IComparable,IComparable,AsserterMod,string)"/>
    void GreaterThan(IComparable? target, IComparable? value, string? details = null);

    /// <summary>Verifies <c>value</c> &gt; <paramref name="target"/>.</summary>
    /// <param name="target">Expected value to compare with.</param>
    /// <param name="value">Actual value under test needing to match the condition</param>
    /// <inheritdoc cref="IAsserterObject.Is(object,object,AsserterMod,string)"/>
    void GreaterThan(
        IComparable? target,
        IComparable? value,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="GreaterThanOrEqualTo(IComparable,IComparable,AsserterMod,string)"/>
    void GreaterThanOrEqualTo(IComparable? target, IComparable? value, string? details = null);

    /// <summary>Verifies <c>value</c> &gt;= <paramref name="target"/>.</summary>
    /// <inheritdoc cref="GreaterThan(IComparable,IComparable,AsserterMod,string)"/>
    void GreaterThanOrEqualTo(
        IComparable? target,
        IComparable? value,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="GreaterThanOrIs(IComparable,IComparable,AsserterMod,string)"/>
    void GreaterThanOrIs(IComparable? target, IComparable? value, string? details = null);

    /// <summary>Verifies <c>value</c> &gt; <paramref name="target"/> or equals by value.</summary>
    /// <inheritdoc cref="GreaterThan(IComparable,IComparable,AsserterMod,string)"/>
    void GreaterThanOrIs(
        IComparable? target,
        IComparable? value,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="LessThan(IComparable,IComparable,AsserterMod,string)"/>
    void LessThan(IComparable? target, IComparable? value, string? details = null);

    /// <summary>Verifies <c>value</c> &lt; <paramref name="target"/>.</summary>
    /// <inheritdoc cref="GreaterThan(IComparable,IComparable,AsserterMod,string)"/>
    void LessThan(
        IComparable? target,
        IComparable? value,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="LessThanOrEqualTo(IComparable,IComparable,AsserterMod,string)"/>
    void LessThanOrEqualTo(IComparable? target, IComparable? value, string? details = null);

    /// <summary>Verifies value is &lt;= <paramref name="target"/>.</summary>
    /// <inheritdoc cref="GreaterThan(IComparable,IComparable,AsserterMod,string)"/>
    void LessThanOrEqualTo(
        IComparable? target,
        IComparable? value,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="LessThanOrIs(IComparable,IComparable,AsserterMod,string)"/>
    void LessThanOrIs(IComparable? target, IComparable? value, string? details = null);

    /// <summary>Verifies <c>value</c> &lt; <paramref name="target"/> or equals by value.</summary>
    /// <inheritdoc cref="GreaterThan(IComparable,IComparable,AsserterMod,string)"/>
    void LessThanOrIs(
        IComparable? target,
        IComparable? value,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="InRange(IComparable,IComparable,IComparable,AsserterMod,string)"/>
    void InRange(IComparable? min, IComparable? max, IComparable? value, string? details = null);

    /// <summary>Verifies <paramref name="min"/> &lt;= <c>value</c> &lt;= <paramref name="max"/>.</summary>
    /// <param name="min">Inclusive lower bound.</param>
    /// <param name="max">Inclusive upper bound.</param>
    /// <inheritdoc cref="GreaterThan(IComparable,IComparable,AsserterMod,string)"/>
    void InRange(
        IComparable? min,
        IComparable? max,
        IComparable? value,
        AsserterMod? optionConfiguration,
        string? details = null
    );
}
