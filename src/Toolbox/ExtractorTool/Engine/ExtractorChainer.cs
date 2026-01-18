using CreateAndFake.Design;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.ExtractorTool.Engine;

/// <summary>Provides a callback into <see cref="IExtractor"/> to extract child values.</summary>
public sealed class ExtractorChainer
    : ToolChainer<ExtractorChainer, IExtractorEngine, ExtractorOptions, ExtractHint>,
        IExtractorChainer
{
    /// <summary>Flattened internal data.</summary>
    private readonly Dictionary<Type, ISet<object>> _foundContents;

    /// <inheritdoc/>
    public ExtractorChainer(ExtractorOptions options, IExtractorEngine engine)
        : base(options, engine)
    {
        _foundContents = [];
    }

    /// <inheritdoc/>
    private ExtractorChainer(ExtractorOptions options, ExtractorChainer prevChainer)
        : base(options, prevChainer)
    {
        _foundContents = prevChainer._foundContents;
    }

    /// <inheritdoc/>
    protected override ExtractorChainer CreateSubChainer(ExtractorOptions options)
    {
        return new ExtractorChainer(options, this);
    }

    /// <inheritdoc/>
    public IContentMap Extract(object? source, ExtractorMod? optionConfiguration = null)
    {
        _ = InnerExtract(source, optionConfiguration);
        return new ContentMap(_foundContents, optionConfiguration?.Invoke(Options) ?? Options);
    }

    /// <inheritdoc/>
    public bool InnerExtract(object? value, ExtractorMod? optionConfiguration = null)
    {
        try
        {
            return Engine.Extract(value, GetSubChainer(optionConfiguration));
        }
        catch (InsufficientExecutionStackException e)
        {
            throw new InsufficientExecutionStackException(
                $"Ran into infinite generation trying to extract type '{value?.GetType()}'.",
                e
            );
        }
    }

    /// <inheritdoc/>
    public bool AddFoundValue(object value, ExtractorMod? optionConfiguration = null)
    {
        if (value == null)
        {
            return false;
        }

        Type keyType = value.GetType();
        if (!_foundContents.TryGetValue(keyType, out ISet<object>? data))
        {
            data = new HashSet<object>(Options.Valuer);
            _foundContents.Add(keyType, data);
        }

        return data.Add(value);
    }

    /// <inheritdoc/>
    public IExtractor WithOptions(ExtractorMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration);
        return new ExtractorChainer(optionConfiguration.Invoke(Options), this);
    }
}
