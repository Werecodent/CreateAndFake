using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Tooling;

namespace Werecodent.CreateAndFake.ExtractorTool.Engine;

/// <summary>Provides a callback into <see cref="IExtractor"/> to extract child values.</summary>
public sealed class ExtractorChainer
    : ToolChainer<ExtractorChainer, IExtractorEngine, ExtractorOptions, IExtractHint>,
        IExtractorChainer
{
    /// <summary>Flattened internal data.</summary>
    private readonly ISet<object> _foundContents;

    /// <summary>Flattened internal data.</summary>
    private readonly IAsyncSet<object> _asyncFoundContents;

    /// <summary>First context used.</summary>
    private bool? _isSync;

    /// <inheritdoc/>
    public ExtractorChainer(ExtractorOptions options, IExtractorEngine engine)
        : base(options, engine)
    {
        _foundContents = new HashSet<object>(Options.Valuer);
        _asyncFoundContents = new AsyncHashSet<object>(Options.Valuer);
        _isSync = null;
    }

    /// <inheritdoc/>
    private ExtractorChainer(ExtractorOptions options, ExtractorChainer prevChainer)
        : base(options, prevChainer)
    {
        _foundContents = prevChainer._foundContents;
        _asyncFoundContents = prevChainer._asyncFoundContents;
        _isSync = prevChainer._isSync;
    }

    /// <inheritdoc/>
    protected override ExtractorChainer CreateSubChainer(ExtractorOptions subOptions)
    {
        return new ExtractorChainer(subOptions, this);
    }

    /// <inheritdoc/>
    public IContentMap Extract(object? source, ExtractorMod? optionConfiguration = null)
    {
        _ = InnerExtract(source, optionConfiguration);
        return new ContentMap(_foundContents, optionConfiguration?.Invoke(Options) ?? Options);
    }

    /// <inheritdoc/>
    public async Task<IAsyncContentMap> ExtractAsync(
        object? source,
        CancellationToken canceler,
        ExtractorMod? optionConfiguration = null
    )
    {
        _ = await InnerExtractAsync(source, canceler, optionConfiguration).ConfigureAwait(false);

        return new AsyncContentMap(
            _asyncFoundContents,
            optionConfiguration?.Invoke(Options) ?? Options
        );
    }

    /// <inheritdoc/>
    public bool InnerExtract(object? value, ExtractorMod? optionConfiguration = null)
    {
        return Engine.Extract(value, GetSubChainer(optionConfiguration));
    }

    /// <inheritdoc/>
    public Task<bool> InnerExtractAsync(
        object? value,
        CancellationToken canceler,
        ExtractorMod? optionConfiguration = null
    )
    {
        return Engine.ExtractAsync(value, GetSubChainer(optionConfiguration), canceler);
    }

    /// <inheritdoc/>
    public bool AddFoundValue(object value, ExtractorMod? optionConfiguration = null)
    {
        if (value == null)
        {
            return false;
        }

        if (_isSync == false)
        {
            throw new MismatchedAccessException(
                value,
                "Cannot add values in synchronous context once asynchronous content has been established."
            );
        }
        _isSync = true;

        return _foundContents.Add(value);
    }

    /// <inheritdoc/>
    public async Task<bool> AddFoundValueAsync(
        object value,
        CancellationToken canceler,
        ExtractorMod? optionConfiguration = null
    )
    {
        if (value == null)
        {
            return false;
        }

        if (_isSync == true)
        {
            throw new MismatchedAccessException(
                value,
                "Cannot add values in asynchronous context once synchronous content has been established."
            );
        }
        _isSync = false;

        return await _asyncFoundContents.AddAsync(value, canceler).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public IExtractor WithOptions(ExtractorMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration);
        return new ExtractorChainer(optionConfiguration.Invoke(Options), this);
    }
}
