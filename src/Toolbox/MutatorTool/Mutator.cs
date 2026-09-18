using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.MutatorTool.Engine;

namespace Werecodent.CreateAndFake.MutatorTool;

/// <inheritdoc cref="IMutator"/>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
public sealed class Mutator(MutatorOptions options) : IMutator
{
    /// <inheritdoc cref="IMutatorEngine"/>
    private static readonly MutatorEngine _Engine = new();

    /// <inheritdoc/>
    public MutatorOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public IEnumerable<Type> SupportedTypes => _Engine.SupportedTypes;

    /// <inheritdoc/>
    public T Variant<T>(T instance, MutatorMod? optionConfiguration = null)
    {
        return new MutatorChainer(Options, _Engine).Variant(instance, optionConfiguration);
    }

    /// <inheritdoc/>
    public object Variant(Type type, object? instance, MutatorMod? optionConfiguration = null)
    {
        return new MutatorChainer(Options, _Engine).Variant(type, instance, optionConfiguration);
    }

    /// <inheritdoc/>
    public Task<T> VariantAsync<T>(
        T instance,
        CancellationToken canceler,
        MutatorMod? optionConfiguration = null
    )
    {
        return new MutatorChainer(Options, _Engine).VariantAsync(
            instance,
            canceler,
            optionConfiguration
        );
    }

    /// <inheritdoc/>
    public Task<object> VariantAsync(
        Type type,
        object? instance,
        CancellationToken canceler,
        MutatorMod? optionConfiguration = null
    )
    {
        return new MutatorChainer(Options, _Engine).VariantAsync(
            type,
            instance,
            canceler,
            optionConfiguration
        );
    }

    /// <inheritdoc/>
    public T VariantOf<T>(IEnumerable<T?> instances, MutatorMod? optionConfiguration = null)
    {
        return new MutatorChainer(Options, _Engine).VariantOf(instances, optionConfiguration);
    }

    /// <inheritdoc/>
    public object VariantOf(
        Type type,
        IEnumerable<object?> instances,
        MutatorMod? optionConfiguration = null
    )
    {
        return new MutatorChainer(Options, _Engine).VariantOf(type, instances, optionConfiguration);
    }

    /// <inheritdoc/>
    public Task<T> VariantOfAsync<T>(
        IEnumerable<T?> instances,
        CancellationToken canceler,
        MutatorMod? optionConfiguration = null
    )
    {
        return new MutatorChainer(Options, _Engine).VariantOfAsync(
            instances,
            canceler,
            optionConfiguration
        );
    }

    /// <inheritdoc/>
    public Task<object> VariantOfAsync(
        Type type,
        IEnumerable<object?> instances,
        CancellationToken canceler,
        MutatorMod? optionConfiguration = null
    )
    {
        return new MutatorChainer(Options, _Engine).VariantOfAsync(
            type,
            instances,
            canceler,
            optionConfiguration
        );
    }

    /// <inheritdoc/>
    public T Unique<T>(T instance, MutatorMod? optionConfiguration = null)
    {
        return new MutatorChainer(Options, _Engine).Unique(instance, optionConfiguration);
    }

    /// <inheritdoc/>
    public object Unique(Type type, object? instance, MutatorMod? optionConfiguration = null)
    {
        return new MutatorChainer(Options, _Engine).Unique(type, instance, optionConfiguration);
    }

    /// <inheritdoc/>
    public Task<T> UniqueAsync<T>(
        T instance,
        CancellationToken canceler,
        MutatorMod? optionConfiguration = null
    )
    {
        return new MutatorChainer(Options, _Engine).UniqueAsync(
            instance,
            canceler,
            optionConfiguration
        );
    }

    /// <inheritdoc/>
    public Task<object> UniqueAsync(
        Type type,
        object? instance,
        CancellationToken canceler,
        MutatorMod? optionConfiguration = null
    )
    {
        return new MutatorChainer(Options, _Engine).UniqueAsync(
            type,
            instance,
            canceler,
            optionConfiguration
        );
    }

    /// <inheritdoc/>
    public T UniqueOf<T>(IEnumerable<T?> instances, MutatorMod? optionConfiguration = null)
    {
        return new MutatorChainer(Options, _Engine).UniqueOf(instances, optionConfiguration);
    }

    /// <inheritdoc/>
    public object UniqueOf(
        Type type,
        IEnumerable<object?> instances,
        MutatorMod? optionConfiguration = null
    )
    {
        return new MutatorChainer(Options, _Engine).UniqueOf(type, instances, optionConfiguration);
    }

    /// <inheritdoc/>
    public Task<T> UniqueOfAsync<T>(
        IEnumerable<T?> instances,
        CancellationToken canceler,
        MutatorMod? optionConfiguration = null
    )
    {
        return new MutatorChainer(Options, _Engine).UniqueOfAsync(
            instances,
            canceler,
            optionConfiguration
        );
    }

    /// <inheritdoc/>
    public Task<object> UniqueOfAsync(
        Type type,
        IEnumerable<object?> instances,
        CancellationToken canceler,
        MutatorMod? optionConfiguration = null
    )
    {
        return new MutatorChainer(Options, _Engine).UniqueOfAsync(
            type,
            instances,
            canceler,
            optionConfiguration
        );
    }

    /// <inheritdoc/>
    public bool Modify(object? instance, MutatorMod? optionConfiguration = null)
    {
        return new MutatorChainer(Options, _Engine).Modify(instance, optionConfiguration);
    }

    /// <inheritdoc/>
    public IMutator WithOptions(MutatorMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration);
        return new Mutator(optionConfiguration.Invoke(Options));
    }
}
