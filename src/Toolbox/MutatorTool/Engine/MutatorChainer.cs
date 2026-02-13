using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.MutatorTool.Engine;

/// <inheritdoc cref="IMutatorChainer"/>
public sealed class MutatorChainer
    : ToolChainer<MutatorChainer, IMutatorEngine, MutatorOptions, IMutateHint>,
        IMutatorChainer
{
    /// <summary>Tracks recursion stack to prevent infinite loops.</summary>
    private readonly ISet<object?> _modifyHistory;

    /// <inheritdoc/>
    public MutatorChainer(MutatorOptions options, IMutatorEngine engine)
        : base(options, engine)
    {
        _modifyHistory = new HashSet<object?>(ReferenceComparer.Use);
    }

    /// <inheritdoc/>
    private MutatorChainer(MutatorOptions options, MutatorChainer prevChainer)
        : base(options, prevChainer)
    {
        _modifyHistory = prevChainer._modifyHistory;
    }

    /// <inheritdoc/>
    protected override MutatorChainer CreateSubChainer(MutatorOptions subOptions)
    {
        return new MutatorChainer(subOptions, this);
    }

    /// <inheritdoc/>
    public T Variant<T>(T instance, MutatorMod? optionConfiguration = null)
    {
        return (T)Variant(typeof(T), instance, optionConfiguration);
    }

    /// <inheritdoc/>
    public object Variant(Type type, object? instance, MutatorMod? optionConfiguration = null)
    {
        return Engine.Variant(type, instance, GetSubChainer(optionConfiguration));
    }

    /// <inheritdoc/>
    public T VariantOf<T>(IEnumerable<T?> instances, MutatorMod? optionConfiguration = null)
    {
        return (T)VariantOf(typeof(T), instances.Cast<object>(), optionConfiguration);
    }

    /// <inheritdoc/>
    public object VariantOf(
        Type type,
        IEnumerable<object?> instances,
        MutatorMod? optionConfiguration = null
    )
    {
        return Engine.VariantOf(type, instances, GetSubChainer(optionConfiguration));
    }

    /// <inheritdoc/>
    public T Unique<T>(T instance, MutatorMod? optionConfiguration = null)
    {
        return (T)Unique(typeof(T), instance, optionConfiguration);
    }

    /// <inheritdoc/>
    public object Unique(Type type, object? instance, MutatorMod? optionConfiguration = null)
    {
        return Engine.Unique(type, instance, GetSubChainer(optionConfiguration));
    }

    /// <inheritdoc/>
    public T UniqueOf<T>(IEnumerable<T?> instances, MutatorMod? optionConfiguration = null)
    {
        return (T)UniqueOf(typeof(T), instances.Cast<object>(), optionConfiguration);
    }

    /// <inheritdoc/>
    public object UniqueOf(
        Type type,
        IEnumerable<object?> instances,
        MutatorMod? optionConfiguration = null
    )
    {
        return Engine.UniqueOf(type, instances, GetSubChainer(optionConfiguration));
    }

    /// <inheritdoc/>
    public bool Modify(object? instance, MutatorMod? optionConfiguration = null)
    {
        if (_modifyHistory.Add(instance))
        {
            try
            {
                return Engine.Modify(instance, GetSubChainer(optionConfiguration));
            }
            finally
            {
                _ = _modifyHistory.Remove(instance);
            }
        }
        else
        {
            return false;
        }
    }

    /// <inheritdoc/>
    public IMutator WithOptions(MutatorMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration);
        return new MutatorChainer(optionConfiguration.Invoke(Options), this);
    }
}
